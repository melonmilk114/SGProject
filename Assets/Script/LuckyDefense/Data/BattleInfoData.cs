using System.Collections.Generic;
using UnityEngine.Serialization;

namespace LuckyDefense
{
#if UNITY_EDITOR
    [System.Serializable]
#endif
    public class BattleInfoData : Melon.Data
    {
        public long gold = 0;
        public long towerSummonCostGold = 100;
        public int towerSummonDefaultLevel = 1;
        public Dictionary<long, long> towerStatusLevel = new Dictionary<long, long>();
        
        public long nowMonsterCount = 0;
        public long maxMonsterCount = 30;
        
        public Observable<BattleInfoData> updateBattleInfoData = null;
        
        public override void InitData()
        {
            gold = 10000;
            towerSummonDefaultLevel = 1;
            
            nowMonsterCount = 0;
            maxMonsterCount = 30;
        }

        public override void ResetData()
        {
            gold = 10000;
            towerSummonDefaultLevel = 1;
            
            nowMonsterCount = 0;
            maxMonsterCount = 30;
        }

        public void PlusGold(long inGold)
        {
            gold += inGold;
            updateBattleInfoData?.NotifyAll(this);
        }
        public void MinusGold(long inGold)
        {
            gold -= inGold;
            if (gold < 0)
                gold = 0;
            
            updateBattleInfoData?.NotifyAll(this);
        }

        public bool IsGoldEnough(long inGold)
        {
            return gold >= inGold;
        }

        public long GetTowerStatusLevel(long inTowerGrade)
        {
            towerStatusLevel.TryAdd(inTowerGrade, 1);
            return towerStatusLevel[inTowerGrade];
        }
        
        public void SetNowMonsterCount(long inCount)
        {
            nowMonsterCount = inCount;
            updateBattleInfoData?.NotifyAll(this);
        }
    }
}