using UnityEngine;
using Melon;
using UnityEngine.Serialization;

namespace LuckyDefense
{
    public class BattleService : GameElement
    , ITargetObjectReceiver<DataManager>
    , ITargetObjectReceiver<TableDataManager>
    {
        public MonsterTableData monsterTableData = null;
        public MissileTableData missileTableData = null;
        public TowerTableData towerTableData = null;
        
        public TowerData towerData = null;
        public BattleInfoData battleInfoData = null;

        #region ITargetObjectReceiver<DataManager>
        DataManager ITargetObjectReceiver<DataManager>.GetTargetObject { get; set; }
        public void SetTargetObject(DataManager inObject)
        {
            ((ITargetObjectReceiver<DataManager>) this).GetTargetObject = inObject;
        }
        
        public DataManager GetDataManager(System.Action<DataManager> inOnNotNull = null)
        {
            var returnManager = ((ITargetObjectReceiver<DataManager>) this).GetTargetObject;
            if (inOnNotNull != null)
                inOnNotNull.Invoke(returnManager);

            return returnManager;
        }
        #endregion
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
        public void InitService()
        {
            GetDataManager(inMgr =>
            {
                towerData = inMgr.towerData;
                battleInfoData = inMgr.battleInfoData;
            });
            
            GetTableDataManager(inMgr =>
            {
                monsterTableData = inMgr.monsterTableData;
                missileTableData = inMgr.missileTableData;
                towerTableData = inMgr.towerTableData;
            });
        }

        public void GameStart()
        {
            battleInfoData.ResetData();
        }

        public long GetTowerRandomSummon(int inGrade)
        {
            var pickTowerSn = towerTableData.GetRandomPick(inGrade);
            if (pickTowerSn <= 0)
            {
                DebugLogHelper.LogError("pickTowerSn is null");
                return 0;
            }

            return pickTowerSn;
        }

        public int GetTowerSummonDefaultLevel()
        {
            return battleInfoData.towerSummonDefaultLevel;
        }

        public long GetTowerSummonCost()
        {
            return battleInfoData.towerSummonCostGold;
        }

        public bool IsTowerSummonCostEnough()
        {
            return battleInfoData.IsGoldEnough(GetTowerSummonCost());
        }
        
        public void PayTowerSummonCost()
        {
            battleInfoData.MinusGold(GetTowerSummonCost());
        }
        
        public bool IsTowerMergeAvailable(long inSn)
        {
            var towerData = towerTableData.FindTowerData(inSn);
            if (towerData == null)
                return false;

            return towerTableData.IsTowerMergeAvailable(towerData);
        }
  
        public void RefundTowerCost(long inSn)
        {
            var findData = towerTableData.FindTowerData(inSn);
            if (findData != null)
            {
                battleInfoData.PlusGold(findData.remove_reward);
            }
            else
            {
                DebugLogHelper.LogError($"findData null sn : {inSn}");
            }
        }

        public void MonsterDeathReward(long inSn)
        {
            var findData = monsterTableData.FindData(inSn);
            if (findData != null)
            {
                battleInfoData.PlusGold(findData.death_reward);
            }
            else
            {
                DebugLogHelper.LogError($"findData null sn : {inSn}");
            }
        }
        
        public void SetNowMonsterCount(long inCount)
        {
            battleInfoData.SetNowMonsterCount(inCount);
        }

        // MEMO : 미사일 데미지 계산
        public MissileTableDataItem GetTowerMissileData(long? inTowerSn)
        {
            var findTowerData = towerTableData.FindTowerData(inTowerSn);
            if (findTowerData == null)
            {
                DebugLogHelper.LogError($"findTowerData is null {inTowerSn}");
                return null;
            }
            
            var missileData = missileTableData.FindMissileData(findTowerData.missile_sn);
            if (missileData == null)
            {
                DebugLogHelper.LogError($"missileData is null {findTowerData.missile_sn}");
                return null;
            }

            return missileData;
        }
        public long GetTowerMissileDamage(long? inTowerSn, MissileTableDataItem inMissileData)
        {
            var findTowerData = towerTableData.FindTowerData(inTowerSn);
            if (findTowerData == null)
            {
                DebugLogHelper.LogError($"findTowerData is null {inTowerSn}");
                return 0;
            }
            
            var damage = Mathf.Round(inMissileData.damage);
            
            return (long)damage;
        }
    }
}