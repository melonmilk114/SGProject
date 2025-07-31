using System.Collections.Generic;
using Melon;
using UnityEngine;

namespace LuckyDefense
{
#if UNITY_EDITOR
    [System.Serializable]
#endif
    public class MissileTableData : Melon.Data
    {
        public List<MissileTableDataItem> dataList = new List<MissileTableDataItem>();
        
        public override void InitData()
        {
            dataList.Clear();

            LoadData();
        }

        public override void ResetData()
        {
            dataList.Clear();
        }
        
        public void AddData(MissileTableDataItem inData)
        {
            var find = dataList.Find(inFindItem => inFindItem.sn == inData.sn);
            if(find == null)
                dataList.Add(inData);
        }
        
        public void LoadData()
        {
            // json 데이터 파싱
            var dataList = JsonHelper.LoadJsonArrayFromFile<MissileTableDataItem>("LuckyDefense/Table/missile");
            foreach (var data in dataList)
            {
                AddData(data);
            }
        }
        
        public MissileTableDataItem FindMissileData(long inSn)
        {
            return dataList[Random.Range(0, dataList.Count)];
        }
    }
    
#if UNITY_EDITOR
    [System.Serializable]
#endif
    public class MissileTableDataItem
    {
        public long sn = 0;
        public float speed = 5f;
        public string destroyHandlerType = "LuckyDefense.MissileDestroyHandler";
        public string moveHandlerType = "LuckyDefense.MissileMoveHandler";
    }
}