using System;
using System.Collections.Generic;
using System.Linq;
using Melon;
using UnityEngine;
using SimpleJSON;
using UnityEngine.Serialization;

namespace LuckyDefense
{
#if UNITY_EDITOR
    [System.Serializable]
#endif
    public class TowerLevelTableData : Melon.Data
    {
        public List<TowerLevelTableDataItem> dataList = new List<TowerLevelTableDataItem>();
        
        public override void InitData()
        {
            dataList.Clear();

            LoadData();
        }

        public override void ResetData()
        {
            dataList.Clear();
        }
        
        public void AddData(TowerLevelTableDataItem inData)
        {
            var find = dataList.Find(inFindItem => inFindItem.sn == inData.sn);
            if (find == null)
            {
                inData.InitData();
                dataList.Add(inData);
            }
        }

        public void LoadData()
        {
            // json 데이터 파싱
            var dataList = JsonHelper.LoadJsonArrayFromFile<TowerLevelTableDataItem>("LuckyDefense/Table/tower_level");
            foreach (var data in dataList)
            {
                AddData(data);
            }
            
            // 데이터 검사 필요함
            // 
        }
        
        public List<TowerLevelTableDataItem> FindLevelDataListByGroupSn(long? inGroupSn)
        {
            if(inGroupSn == null)
                return null;
            return dataList.Where(inFindItem => inFindItem.group_sn == inGroupSn).OrderBy(inOrderItem => inOrderItem.level).ToList();
        }
    }
#if UNITY_EDITOR
    [System.Serializable]
#endif
    public class TowerLevelTableDataItem
    {
        public long sn = 0;
        public long group_sn = 0;
        public long level = 0;
 
        // 스탯 관련
        public string stat_type = "";
        public string stat_modifier_type = "";
        public float stat_modifier_value = 0;
        
        public STAT_TYPE stat_type_enum = STAT_TYPE.NONE;
        public STAT_MODIFIER_TYPE stat_modifier_type_enum = STAT_MODIFIER_TYPE.NONE;

        public void InitData()
        {
            stat_type_enum = stat_type.EnumParse<STAT_TYPE>();
            stat_modifier_type_enum = stat_modifier_type.EnumParse<STAT_MODIFIER_TYPE>();
        }
    }
}