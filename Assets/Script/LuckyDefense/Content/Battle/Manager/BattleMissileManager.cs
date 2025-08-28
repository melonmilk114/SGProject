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
        public GameObject missilesParent;
        public GameObject damageParent;
        
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

        public List<MissileObject> FindDestroyReadyMissiles()
        {
            return _missiles.FindAll(inItem => inItem.isDestroyReady);
        }

        public void InitContent()
        {
            
        }
        
        
        public void UpdateContent(float inDeltaTime)
        {
            _missiles.ForEach(inItem => inItem.UpdateObject(inDeltaTime));
        }
        
        public void CreateMissile(TowerObject inTower, MonsterObject inMonsterObject, MissileTableDataItem inMissileTableDataItem, long inMissileDamage)
        {
            // 미사일 생성
            MissileObject missile = ObjectPoolManager.Instance.DequeuePool<MissileObject>(missilesParent);
            if (missile == null)
            {
                Debug.LogError($"{this} 미사일 오브젝트가 null 입니다.");
                return;
            }
            
            missile.Init(inMissileTableDataItem, inMonsterObject);
            missile.SetDamage(inMissileDamage);
            inMonsterObject.PreDamage(missile);
        
            missile.onHitMonster = OnMissileHitMonster;
            
            missile.transform.position.Normalize();
            missile.transform.position = inTower.transform.position;
            _missiles.Add(missile);
        }
        
        
        private void OnMissileHitMonster(MissileObject inMissile, MonsterObject inMonster)
        {
            inMonster.MissileHit(inMissile);
        }
        
        public void CreateDamage(long inMissileDamage, Vector3 inPos)
        {
            // 미사일 생성
            DamageObject damage = ObjectPoolManager.Instance.DequeuePool<DamageObject>(damageParent);
            if (damage == null)
            {
                Debug.LogError($"{this} 데미지 오브젝트가 null 입니다.");
                return;
            }
            
            damage.transform.position.Normalize();
            damage.transform.position = inPos;
            damage.SetDamage(inMissileDamage);
            
        }
        
        public void MissileDestroy(MissileObject inMissile)
        {
            inMissile.MissileDestroy(new ActionResult()
            {
                onSuccess = () =>
                {
                    CreateDamage(inMissile.GetDamage(), inMissile.transform.position);
                    _missiles.Remove(inMissile);
                    ObjectPoolManager.Instance.EnqueuePool(inMissile);
                }
            });
        }
    }
}