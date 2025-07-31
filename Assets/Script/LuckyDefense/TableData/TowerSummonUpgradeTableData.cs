using System;
using System.Collections.Generic;
using Melon;
using UnityEngine;

namespace LuckyDefense
{
#if UNITY_EDITOR
    [System.Serializable]
#endif
    public class TowerSummonUpgradeData : Melon.Data
    {
        [SerializeField] private List<TowerSummonUpgradeTableDataItem> dataList = new List<TowerSummonUpgradeTableDataItem>();
        public override void InitData()
        {
            dataList.Clear();

            LoadData();
        }

        public override void ResetData()
        {
            
        }
        
        public void AddData(TowerSummonUpgradeTableDataItem inData)
        {
            var find = dataList.Find(inFindItem => inFindItem.sn == inData.sn);
            if(find == null)
                dataList.Add(inData);
        }
        
        public void LoadData()
        {
            // json 데이터 파싱
            var parsingList = JsonHelper.LoadJsonArrayFromFile<TowerSummonUpgradeTableDataItem>("LuckyDefense/Table/tower_summon_upgrade");
            foreach (var data in parsingList)
            {
                AddData(data);
            }
            
            // 데이터 검사 필요함
            // 
        }

        #region FindFunc

        public TowerSummonUpgradeTableDataItem FindDataByLevel(long inLevel)
        {
            return dataList.Find(inFindData => inFindData.level == inLevel);
        }

        #endregion

        public long GetTowerSummonCost(long inLevel)
        {
            var findData = FindDataByLevel(inLevel);
            if (findData != null)
            {
                return findData.summon_cost;
            }
            else
            {
                DebugLogHelper.Log($"findData is null Level {inLevel}", this, "GetTowerSummonCost");
            }

            return 0;
        }
    }
#if UNITY_EDITOR
    [System.Serializable]
#endif
    public class TowerSummonUpgradeTableDataItem
    {
        public long sn = 0;
        public long level = 0;
        public long group = 0;
        public long upgrade_cost = 0;
        public long summon_cost = 0;
    }
}