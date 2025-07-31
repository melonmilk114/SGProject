using UnityEngine;

namespace GridHeroes
{
    public class DataManager : Melon.DataManager
    {
        public override void InitManager()
        {
            base.InitManager();

            AddDataUpdateFunc(rootObj);
        }
        
        public override void InitData()
        {
            base.InitData();
        }

        public override void ResetData()
        {
            base.ResetData();
            
        }
    
        public void AddDataUpdateFunc(GameObject inObj)
        {
            var list = inObj.GetComponentsInChildren<IDataUpdateFunc>(true);

            for (int idx = 0; idx < list.Length; idx++)
            {
                //updateBattleInfoData.AddListener(list[idx]);
            }
        }

        public void RemoveDataUpdateFunc(GameObject inObj)
        {
            var list = inObj.GetComponentsInChildren<IDataUpdateFunc>(true);

            for (int idx = 0; idx < list.Length; idx++)
            {
                //updateBattleInfoData.RemoveListener(list[idx]);
            }
        }
    }
}