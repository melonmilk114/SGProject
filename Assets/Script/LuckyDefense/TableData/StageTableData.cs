using System.Collections.Generic;
using Melon;
using UnityEngine;

namespace LuckyDefense
{
#if UNITY_EDITOR
    [System.Serializable]
#endif
    public class StageTableData : Melon.Data
    {
        public List<StageTableDataItem> dataList = new List<StageTableDataItem>();
        
        public override void InitData()
        {
            dataList.Clear();

            LoadData();
        }

        public override void ResetData()
        {
            dataList.Clear();
        }
        
        public void AddData(StageTableDataItem inData)
        {
            var find = dataList.Find(inFindItem => inFindItem.sn == inData.sn);
            if(find == null)
                dataList.Add(inData);
        }
        
        public void LoadData()
        {
            // json 데이터 파싱
            var dataList = JsonHelper.LoadJsonArrayFromFile<StageTableDataItem>("LuckyDefense/Table/stage");
            foreach (var data in dataList)
            {
                AddData(data);
            }
        }
        
        public StageTableDataItem FindStageData(long inSn)
        {
            return dataList.Find(inFindItem => inFindItem.sn == inSn);
        }
    }
    
#if UNITY_EDITOR
    [System.Serializable]
#endif
    public class StageTableDataItem
    {
        public long sn = 0;
        public long wave_group = 0;
    }
}