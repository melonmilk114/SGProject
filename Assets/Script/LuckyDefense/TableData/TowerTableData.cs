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
    public class TowerTableData : Melon.Data
    {
        public List<TowerTableDataItem> dataList = new List<TowerTableDataItem>();
        
        public override void InitData()
        {
            dataList.Clear();

            LoadData();
        }

        public override void ResetData()
        {
            dataList.Clear();
        }
        
        public void AddData(TowerTableDataItem inData)
        {
            var find = dataList.Find(inFindItem => inFindItem.sn == inData.sn);
            if(find == null)
                dataList.Add(inData);
        }

        public void LoadData()
        {
            // json 데이터 파싱
            var dataList = JsonHelper.LoadJsonArrayFromFile<TowerTableDataItem>("LuckyDefense/Table/tower");
            foreach (var data in dataList)
            {
                AddData(data);
            }
            
            // 데이터 검사 필요함
            // 
        }


        public TowerTableDataItem FindTowerData(long? inSn)
        {
            return dataList.Find(inFindData => inFindData.sn == inSn);
        }
        
        public List<TowerTableDataItem> FindTowerDataListByGrade(long inGrade)
        {
            return dataList.FindAll(inFindItem => inFindItem.grade == inGrade);
        }

        public List<TowerTableDataItem> GetAllTowerDataList()
        {
            return dataList;
        }
        
        public long GetRandomPick(long inGrade)
        {
            var findDataList = FindTowerDataListByGrade(inGrade);
            
            int totalWeight = 0;
            findDataList.ForEach(inForItem =>
            {
                totalWeight += inForItem.summon_weight;
            });
            
            int randomWeight = Random.Range(0, totalWeight);
            int currWeight = 0;

            for (int idx = 0; idx < findDataList.Count; idx++)
            {
                currWeight += findDataList[idx].summon_weight;
        
                if (randomWeight < currWeight) 
                    return findDataList[idx].sn;
            }

            return 0;
        }

        public bool IsTowerMergeAvailable(TowerTableDataItem inData)
        {
            // 다음 등급이 있는가?
            var findList = FindTowerDataListByGrade(inData.grade + 1);
            return findList.Count > 0;
        }
    }
#if UNITY_EDITOR
    [System.Serializable]
#endif
    public class TowerTableDataItem
    {
        public long sn = 0;
        public int grade = 0;
        public long remove_reward = 0;
        public long missile_sn = 0;
        public float attack_range = 3.0f;
        public int summon_weight = 0;
        public string animator = "";
        
        public long level_group_sn = 0; // 타워 레벨 그룹
        
        // 타워 스탯 (데이터화가 필요해 보임)
        public float attack;
        public float attack_speed;
        public float critical_rate;
    }
}