using System.Collections.Generic;
using System.Linq;
using Melon;
using UnityEngine;

namespace LuckyDefense
{
#if UNITY_EDITOR
    [System.Serializable]
#endif
    public class TowerSummonTableData : Melon.Data
    {
        [SerializeField] private List<TowerSummonTableDataItem> dataList = new List<TowerSummonTableDataItem>();
        public override void InitData()
        {
            dataList.Clear();

            LoadData();
        }

        public override void ResetData()
        {
            
        }
        
        public void AddData(TowerSummonTableDataItem inData)
        {
            var find = dataList.Find(inFindItem => inFindItem.sn == inData.sn);
            if(find == null)
                dataList.Add(inData);
        }
        
        public void LoadData()
        {
            // json 데이터 파싱
            var parsingList = JsonHelper.LoadJsonArrayFromFile<TowerSummonTableDataItem>("LuckyDefense/Table/tower_summon");
            foreach (var data in parsingList)
            {
                AddData(data);
            }
            
            // 데이터 검사 필요함
            // 
        }

        public List<TowerSummonTableDataItem> FindDataListByGroup(long inGroup)
        {
            return dataList.FindAll(inFindItem => inFindItem.grade == inGroup);
        }

        public long GetRandomPickGrade(long inGroup)
        {
            var findDataList = FindDataListByGroup(inGroup);
            findDataList = findDataList
                .OrderBy(inItem => inItem.grade)
                .ToList();
            
            int totalWeight = 0;
            findDataList.ForEach(inForItem =>
            {
                totalWeight += inForItem.weight;
            });
            
            int randomWeight = Random.Range(0, totalWeight);
            int currWeight = 0;

            for (int idx = 0; idx < findDataList.Count; idx++)
            {
                currWeight += findDataList[idx].weight;
        
                if (randomWeight < currWeight) 
                    return findDataList[idx].grade;
            }

            return 0;
        }
    }
#if UNITY_EDITOR
    [System.Serializable]
#endif
    public class TowerSummonTableDataItem
    {
        public long sn = 0;
        public long group = 0;
        public long grade = 0;
        public int weight = 0;
    }
}