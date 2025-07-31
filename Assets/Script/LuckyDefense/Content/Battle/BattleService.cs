using UnityEngine;
using LuckyDefense.Interface;
using Melon;
using UnityEngine.Serialization;

namespace LuckyDefense
{
    public class BattleService : GameElement, IBattleService
    , ITargetObjectReceiver<DataManager>
    , ITargetObjectReceiver<TableDataManager>
    {
        public MonsterTableData monsterTableData = null;
        public TowerTableData towerTableData = null;
        public TowerLevelUpgradeTableData towerLevelUpgradeTableData = null;
        public TowerStatusUpgradeTableData towerStatusUpgradeTableData = null;
        public TowerSummonTableData towerSummonTableData = null;
        public TowerSummonUpgradeData towerSummonUpgradeData = null;
        
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
                towerTableData = inMgr.towerTableData;
                towerLevelUpgradeTableData = inMgr.towerLevelUpgradeTableData;
                towerSummonTableData = inMgr.towerSummonTableData;
                towerSummonUpgradeData = inMgr.towerSummonUpgradeData;
            });
        }

        public long GetTowerSummon()
        {
            var findTowerSummonUpgradeData = towerSummonUpgradeData.FindDataByLevel(battleInfoData.towerSummonLevel);
            if (findTowerSummonUpgradeData == null)
            {
                DebugLogHelper.LogError("findTowerSummonUpgradeData is null", this, "GetTowerSummon");
                return 0;
            }
            
            var pickGrade = towerSummonTableData.GetRandomPickGrade(findTowerSummonUpgradeData.group);
            if (pickGrade <= 0)
            {
                DebugLogHelper.LogError("pickGrade is null", this, "GetTowerSummon");
                return 0;
            }
            
            var pickTowerSn = towerTableData.GetRandomPick(pickGrade);
            if (pickTowerSn <= 0)
            {
                DebugLogHelper.LogError("pickTowerSn is null", this, "GetTowerSummon");
                return 0;
            }

            return pickTowerSn;
        }

        public long GetTowerSummonCost()
        {
            return towerSummonUpgradeData.GetTowerSummonCost(battleInfoData.towerSummonLevel);
        }

        public bool IsTowerSummonCostEnough()
        {
            return battleInfoData.IsGoldEnough(GetTowerSummonCost());
        }
        
        public void PayTowerSummonCost()
        {
            battleInfoData.MinusGold(GetTowerSummonCost());
        }

        public long GetTowerTierUpCost(long inSn)
        {
            var findData = towerTableData.FindTowerData(inSn);
            if (findData != null)
            {
                return findData.upgrade_cost;
            }
            else
            {
                DebugLogHelper.LogError($"findData null sn : {inSn}", this, "GetTowerTierUpCost");
            }
            return 0;
        }
        
        public bool IsTowerUpgradeCostEnough(long inSn)
        {
            return battleInfoData.IsGoldEnough(GetTowerTierUpCost(inSn));
        }
        
        public void PayTowerTierUpCost(long inSn)
        {
            battleInfoData.MinusGold(GetTowerTierUpCost(inSn));
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
                DebugLogHelper.LogError($"findData null sn : {inSn}", this, "RefundTowerCost");
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
                DebugLogHelper.LogError($"findData null sn : {inSn}", this, "MonsterDeathReward");
            }
        }

        // MEMO : 미사일 데미지 계산
        public long GetTowerMissileDamage(long inTowerSn)
        {
            var findTowerData = towerTableData.FindTowerData(inTowerSn);
            if (findTowerData == null)
            {
                DebugLogHelper.LogError($"findTowerData is null {inTowerSn}", this, "GetTowerMissileDamage");
                return 0;
            }
            
            var findTowerStatusLevel = battleInfoData.GetTowerStatusLevel(findTowerData.grade);
            var towerDamage = towerLevelUpgradeTableData.GetTowerDamage(inTowerSn, findTowerStatusLevel);

            var towerDamageRate =
                towerStatusUpgradeTableData.GetTowerDamageRate(findTowerData.grade, findTowerStatusLevel);

            var damage = Mathf.Round(towerDamage * towerDamageRate);
            
            return (long)damage;
        }
    }
}