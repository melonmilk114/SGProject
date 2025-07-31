using System;
using System.Collections.Generic;
using Melon;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LuckyDefense
{
#if UNITY_EDITOR
    [System.Serializable]
#endif
    public class MonsterTableData : Melon.Data
    {
        public List<MonsterTableDataItem> dataList = new List<MonsterTableDataItem>();
        
        public override void InitData()
        {
            dataList.Clear();
            LoadData();
        }

        public override void ResetData()
        {
            dataList.Clear();
        }

        public void AddData(MonsterTableDataItem inData)
        {
            var find = dataList.Find(inFindItem => inFindItem.sn == inData.sn);
            if(find == null)
                dataList.Add(inData);
        }

        public void LoadData()
        {
            // json 데이터 파싱
            var dataList = JsonHelper.LoadJsonArrayFromFile<MonsterTableDataItem>("LuckyDefense/Table/monster");
            foreach (var data in dataList)
            {
                AddData(data);
            }
        }

        public MonsterTableDataItem FindData(long inSn)
        {
            return dataList.Find(inFindItem => inFindItem.sn == inSn);
        }
    }
    
#if UNITY_EDITOR
    [System.Serializable]
#endif
    public class MonsterTableDataItem
    {
        public long sn = 0;
        public long hp = 2;
        public float speed = 3;
        public string sprite = "";
        public long death_reward = 0;

        public string deathHandlerType = "LuckyDefense.MonsterDeathHandler";
        public string moveHandlerType = "LuckyDefense.MonsterMoveHandler";
    }
}