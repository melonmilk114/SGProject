using System;
using System.Collections.Generic;
using Melon;
using UnityEngine;
using UnityEngine.Serialization;

namespace LuckyDefense
{
    public class BattleMonsterManager : GameElement
    ,ITargetObjectReceiver<TableDataManager>
    {
        [SerializeField] public List<MonsterObject> _monsters = new List<MonsterObject>();
        
        public List<Transform> monsterWayPoints = new List<Transform>();
        private List<Vector3> _wayPoints = new List<Vector3>();
        
        public GameObject monstersParent;

        public MonsterTableData monsterTableData;
        
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
        
        public void UpdateContent(float inDeltaTime)
        {
            _monsters.ForEach(inItem => inItem.UpdateObject(inDeltaTime));
        }
        
        public void InitContent()
        {
            GetTableDataManager(inMgr =>
            {
                monsterTableData = inMgr.monsterTableData;
            });
            
            _wayPoints.Clear();
            monsterWayPoints.ForEach(inItem => _wayPoints.Add(inItem.position));
        }
        
        public MonsterObject FindAliveMonsterObject()
        {
            return _monsters.Find(inFindItem => inFindItem.state.isPreDead == false);
        }
        
        public MonsterObject FindTargetsInRange(TowerGroupObject inTowerGroup)
        {
            return _monsters.Find(inFindItem =>
            {
                return inFindItem.state.isPreDead == false && inTowerGroup.IsInAttackRange(inFindItem.transform);
            });
        }
        
        public List<MonsterObject> FindDeadMonster()
        {
            return _monsters.FindAll(inFindItem => inFindItem.isDead);
        }
        
        
        public void CreateMonster(long inSn, UIMonsterHpBar inHpBar)
        {
            var tmpMonsterData = monsterTableData.FindData(inSn);
            if (tmpMonsterData == null)
            {
                Debug.LogError($"{this} 적 데이터({inSn})가 null 입니다.");
                return;
            }
            
            MonsterObject monster = ObjectPoolManager.Instance.DequeuePool<MonsterObject>(monstersParent);
            if (monster == null)
            {
                Debug.LogError($"{this} 적 오브젝트가 null 입니다.");
                return;
            }
            
            monster.Init(tmpMonsterData, _wayPoints, inHpBar);

            monster.transform.position = _wayPoints[0];
            _monsters.Add(monster);
        }
        
        public void MonsterDeath(MonsterObject inMonster)
        {
            _monsters.Remove(inMonster);
            ObjectPoolManager.Instance.EnqueuePool(inMonster);
        }
    }
}