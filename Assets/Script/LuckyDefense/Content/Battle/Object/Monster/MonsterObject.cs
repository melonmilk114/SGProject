using System;
using System.Collections.Generic;
using Melon;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace LuckyDefense
{
    public class MonsterObject : GameElement, IObjectPoolUnit
    {
        public MonsterObjectState state;
        public MonsterObjectView view;
        public MonsterTableDataItem tableData = null;
        
        private MonsterMoveHandler _moveHandler;
        private MonsterDeathHandler _deathHandler;
        
        public List<MissileObject> targetMonsterMissiles = new List<MissileObject>();

        public Action<MissileObject> onMissileDestroy = null;
        public Action<MonsterObject> onDeath = null;
        
        public void OnPoolDequeue()
        {
            CommonUtils.AllRemoveComponent<MonsterHandler>(gameObject);
            state.Reset();
            
            targetMonsterMissiles.ForEach(inForItem =>
            {
                onMissileDestroy?.Invoke(inForItem);
            });
        }

        public void OnPoolEnqueue()
        {
            
        }

        public void Init(MonsterTableDataItem inData, List<Vector3> inWayPoints, UIMonsterHpBar inHpBar)
        {
            tableData = inData;
            
            _deathHandler = CommonUtils.AddComponent<MonsterDeathHandler>(gameObject, tableData.deathHandlerType);
            _moveHandler = CommonUtils.AddComponent<MonsterMoveHandler>(gameObject, tableData.moveHandlerType);
            
            if (_deathHandler == null || _moveHandler == null)
            {
                Debug.LogError($"[MonsterObject] 핸들러가 누락되었습니다. monsterSn={tableData.sn}");
                return;
            }

            var compList = GetComponents<MonsterHandler>();
            for (var idx = 0; idx < compList.Length; idx++)
            {
                compList[idx].Setup(this);
            }
            
            _moveHandler.SetWayPoint(inWayPoints);
            
            state.Init(tableData);
            view.Init(tableData);
            view.AttachHpBar(inHpBar);
            view.SetHp(state.hpRatio);
        }

        public void UpdateObject(float inDeltaTime)
        {
            _moveHandler?.Move(inDeltaTime);
        }

        
        // 발사된 미사일을 등록 (예측 피해 계산)
        public void PreDamage(MissileObject inObj)
        {
            state?.PreDamage(inObj.GetDamage());
            targetMonsterMissiles.Add(inObj);
        }

        // 실제 충돌된 미사일 처리
        public void MissileHit(MissileObject inObj)
        {
            state?.MissileHit(inObj.GetDamage());
            view?.SetHp(state.hpRatio);
            
            targetMonsterMissiles.Remove(inObj);
            
            // 미사일 파괴
            onMissileDestroy?.Invoke(inObj);
 
            if (state?.isDead == true)
            {
                _deathHandler.Death(new ActionResult()
                {
                    onSuccess = () =>
                    {
                        onDeath?.Invoke(this);
                        // UI 사망
                        view?.Death();
                    }
                });
            }
        }

        

        
    }
}