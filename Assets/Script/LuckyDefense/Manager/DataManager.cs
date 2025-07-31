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
        
        public DataSubject<BattleInfoData> updateBattleInfoData = new DataSubject<BattleInfoData>();
        
        public override void InitManager()
        {
            base.InitManager();
            
            battleInfoData.updateBattleInfoData = updateBattleInfoData;
            
            AddObserverObj(rootObj);
            
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
    
        public void AddObserverObj(GameObject inObj)
        {
            var list = inObj.GetComponentsInChildren<IDataObserver>(true);

            for (int idx = 0; idx < list.Length; idx++)
            {
                updateBattleInfoData.AddObserverObj(list[idx]);
            }
        }

        public void RemoveObserverObj(GameObject inObj)
        {
            var list = inObj.GetComponentsInChildren<IDataObserver>(true);

            for (int idx = 0; idx < list.Length; idx++)
            {
                updateBattleInfoData.RemoveObserverObj(list[idx]);
            }
        }
    }
}