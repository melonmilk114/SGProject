using System.Collections.Generic;
using Melon;
using UnityEngine;
using UnityEngine.Serialization;

namespace LuckyDefense
{
#if UNITY_EDITOR
    [System.Serializable]
#endif
    public class WaveTableData : Melon.Data
    {
        public List<WaveTableDataItem> dataList = new List<WaveTableDataItem>();
        
        public override void InitData()
        {
            dataList.Clear();

            LoadData();
        }

        public override void ResetData()
        {
            dataList.Clear();
        }
        
        public void AddData(WaveTableDataItem inData)
        {
            var find = dataList.Find(inFindItem => inFindItem.sn == inData.sn);
            if(find == null)
                dataList.Add(inData);
        }
        
        public void LoadData()
        {
            // json 데이터 파싱
            var dataList = JsonHelper.LoadJsonArrayFromFile<WaveTableDataItem>("LuckyDefense/Table/wave");
            foreach (var data in dataList)
            {
                AddData(data);
            }
        }
        
        public List<WaveTableDataItem> FindWaveDataList(long inGroupSn)
        {
            return dataList.FindAll(inFindItem => inFindItem.group == inGroupSn);
        }
        
        public WaveTableDataItem FindWaveData(long inSn)
        {
            return dataList.Find(inFindItem => inFindItem.sn == inSn);
        }
    }
    
#if UNITY_EDITOR
    [System.Serializable]
#endif
    public class WaveTableDataItem
    {
        // 적이 섞여서 나오지 않는다
        public long sn = 0;
        public long group = 0;
        public long monster_sn = 0;
        public long monster_count = 0;
        public float interval = 0f;
    }
}