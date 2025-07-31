using System;
using System.Collections.Generic;
using Melon;
using UnityEngine;

namespace LuckyDefense
{
    public class BattleMissileManager : GameElement
    ,ITargetObjectReceiver<TableDataManager> 
    {
        [SerializeField] private List<MissileObject> _missiles = new List<MissileObject>();
        [SerializeField] private List<MissileObject> _clearMissiles = new List<MissileObject>();
        public GameObject missilesParent;
        
        public MissileTableData missileTableData;
        
        #region ITargetObjectReceiver<TableDataManager>
        TableDataManager ITargetObjectReceiver<TableDataManager>.GetTargetObject { get; set; }
        public void SetTargetObject(TableDataManager inObject)
        {
            ((ITargetObjectReceiver<TableDataManager>) this).GetTargetObject = inObject;
        }
        
        public TableDataManager GetTableDataManager(System.Action<TableDataManager> inOnNotNull = null)
        {
            var returnManager = ((ITargetObjectReceiver<TableDataManager>) this).GetTargetObject;
            if (inOnNotNull != null)
                inOnNotNull.Invoke(returnManager);

            return returnManager;
        }
        #endregion

        public void InitContent()
        {
            missileTableData = GetTableDataManager()?.missileTableData;
        }
        
        
        public void UpdateContent(float inDeltaTime)
        {
            _missiles.ForEach(inItem => inItem.UpdateObject(inDeltaTime));
            
            // 미사일 삭제
            if (_clearMissiles.Count > 0)
            {
                _clearMissiles.ForEach(inItem =>
                {
                    _missiles.Remove(inItem);
                });
                
                _clearMissiles.Clear();
            }
        }
        
        public void CreateMissile(TowerGroupObject inTowerGroup, MonsterObject inMonsterObject)
        {
            // 발사해야하는 미사일 데이터 가져오기
            var tmpMissileData = missileTableData.FindMissileData(inTowerGroup.TowerTableData.missile_sn);
            if (tmpMissileData == null)
            {
                Debug.LogError($"{this} 미사일 데이터({inTowerGroup.TowerTableData.missile_sn})가 null 입니다.");
                return;
            }
            
            // 미사일 생성
            MissileObject missile = ObjectPoolManager.Instance.DequeuePool<MissileObject>(missilesParent);
            if (missile == null)
            {
                Debug.LogError($"{this} 미사일 오브젝트가 null 입니다.");
                return;
            }
            
            missile.Init(tmpMissileData, inMonsterObject);
            inMonsterObject.PreDamage(missile);
        
            missile.onHitMonster = OnMissileHitMonster;
            
            missile.transform.position = inTowerGroup.transform.position;
            _missiles.Add(missile);
        }
        
        
        private void OnMissileHitMonster(MissileObject inMissile, MonsterObject inMonster)
        {
            inMonster.MissileHit(inMissile);
        }
        
        public void MissileDestroy(MissileObject inMissile)
        {
            inMissile.MissileDestroy(new ActionResult()
            {
                onSuccess = () =>
                {
                    _clearMissiles.Add(inMissile);
                    ObjectPoolManager.Instance.EnqueuePool(inMissile);
                }
            });
        }
    }
}