using System.Collections.Generic;
using Melon;

namespace LuckyDefense
{
#if UNITY_EDITOR
    [System.Serializable]
#endif
    public class TowerStatusUpgradeTableData : Melon.Data
    {
        // MEMO : 인게임 도중에 강화 해서 능력치가 강화 되는 데이터
        public List<TowerStatusUpgradeTableDataItem> dataList = new List<TowerStatusUpgradeTableDataItem>();
        
        public override void InitData()
        {
            dataList.Clear();

            LoadData();
        }

        public override void ResetData()
        {
            dataList.Clear();
        }

        public TowerStatusUpgradeTableDataItem FindDataByTowerGradeLevel(long inTowerGrade, long inTowerLevel)
        {
            return dataList.Find(inFindItem => inFindItem.grade == inTowerGrade &&  inFindItem.level == inTowerLevel);
        }
        
        public void AddData(TowerStatusUpgradeTableDataItem inData)
        {
            var find = dataList.Find(inFindItem => inFindItem.sn == inData.sn);
            if(find == null)
                dataList.Add(inData);
        }

        public void LoadData()
        {
            // json 데이터 파싱
            var dataList = JsonHelper.LoadJsonArrayFromFile<TowerStatusUpgradeTableDataItem>("LuckyDefense/Table/tower_status_upgrade");
            foreach (var data in dataList)
            {
                AddData(data);
            }
            
            // 데이터 검사 필요함
            // 
        }
        
        public float GetTowerDamageRate(long inTowerGrade, long inTowerLevel)
        {
            var findData = FindDataByTowerGradeLevel(inTowerGrade, inTowerLevel);
            if (findData == null)
            {
                DebugLogHelper.LogError($"findData is null {inTowerGrade} {inTowerLevel}", this, "GetTowerDamageRate");
                return 1f;
            }

            return findData.attack_rate;
        }
    }
    
#if UNITY_EDITOR
    [System.Serializable]
#endif
    public class TowerStatusUpgradeTableDataItem
    {
        public long sn = 0;
        public long grade = 0;
        public long level = 0;
        public long level_up_cost = 0;
        public float attack_rate = 0;
    }
}