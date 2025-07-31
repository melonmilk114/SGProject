using System.Collections.Generic;

namespace LuckyDefense
{
#if UNITY_EDITOR
    [System.Serializable]
#endif
    public class TowerData : Melon.Data
    {
        public List<TowerDataItem> dataList = new List<TowerDataItem>();
        
        public override void InitData()
        {
            dataList.Add(new TowerDataItem()
            {
                tower_sn = 1,
                level = 1,
            });
            dataList.Add(new TowerDataItem()
            {
                tower_sn = 2,
                level = 1,
            });
            dataList.Add(new TowerDataItem()
            {
                tower_sn = 3,
                level = 1,
            });
        }

        public override void ResetData()
        {
            
        }
    }
#if UNITY_EDITOR
    [System.Serializable]
#endif
    public class TowerDataItem
    {
        public long tower_sn = 0;
        public long level = 0;
    }
} 