using System.Collections.Generic;
using Melon;

namespace LuckyDefense
{
    public class TowerLevelUpgradeTableData : Melon.Data
    {
        public List<TowerLevelUpgradeTableDataItem> dataList = new List<TowerLevelUpgradeTableDataItem>();
        
        public override void InitData()
        {
            dataList.Clear();

            LoadData();
        }

        public override void ResetData()
        {
            dataList.Clear();
        }

        public TowerLevelUpgradeTableDataItem FindDataByTowerSnLevel(long inTowerSn, long inTowerLevel)
        {
            return dataList.Find(inFindData => inFindData.tower_sn == inTowerSn && inFindData.level == inTowerLevel);
        }
        
        public void AddData(TowerLevelUpgradeTableDataItem inData)
        {
            var find = dataList.Find(inFindItem => inFindItem.sn == inData.sn);
            if(find == null)
                dataList.Add(inData);
        }

        public void LoadData()
        {
            // json 데이터 파싱
            var dataList = JsonHelper.LoadJsonArrayFromFile<TowerLevelUpgradeTableDataItem>("LuckyDefense/Table/tower_level_upgrade");
            foreach (var data in dataList)
            {
                AddData(data);
            }
            
            // 데이터 검사 필요함
            // 
        }
        
        public long GetTowerDamage(long inTowerSn, long inTowerLevel)
        {
            var findData = FindDataByTowerSnLevel(inTowerSn, inTowerLevel);
            if (findData == null)
            {
                DebugLogHelper.LogError($"findData is null {inTowerSn} {inTowerLevel}", this, "GetTowerDamage");
                return 0;
            }

            return findData.attack;
        }
    }
#if UNITY_EDITOR
    [System.Serializable]
#endif
    public class TowerLevelUpgradeTableDataItem
    {
        public long sn = 0;
        public long tower_sn = 0;
        public long level = 0;
        public long required_count = 0;
        public long attack = 0;
    }
}