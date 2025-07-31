using System;
using Melon;
using UnityEngine;
using UnityEngine.Serialization;

namespace LuckyDefense
{
    public class DataManager : Melon.DataManager
    {
        public TowerData towerData = new TowerData();
        public BattleInfoData battleInfoData = new BattleInfoData();
        
        public DataUpdateFunc<BattleInfoData> updateBattleInfoData = new DataUpdateFunc<BattleInfoData>();
        
        public override void InitManager()
        {
            base.InitManager();
            
            battleInfoData.updateBattleInfoData = updateBattleInfoData;
            
            AddDataUpdateFunc(rootObj);
            
            TargetObjectReceiver.DoTargetObjectInject(this, rootObj);
        }
        
        public override void InitData()
        {
            base.InitData();
            
            towerData.InitData();
            battleInfoData.InitData();
        }

        public override void ResetData()
        {
            base.ResetData();

            towerData.ResetData();
            battleInfoData.ResetData();
        }
    
        public void AddDataUpdateFunc(GameObject inObj)
        {
            var list = inObj.GetComponentsInChildren<IDataUpdateFunc>(true);

            for (int idx = 0; idx < list.Length; idx++)
            {
                updateBattleInfoData.AddListener(list[idx]);
            }
        }

        public void RemoveDataUpdateFunc(GameObject inObj)
        {
            var list = inObj.GetComponentsInChildren<IDataUpdateFunc>(true);

            for (int idx = 0; idx < list.Length; idx++)
            {
                updateBattleInfoData.RemoveListener(list[idx]);
            }
        }
    }
}