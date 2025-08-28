using System.Collections.Generic;

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
        
        public Observable<BattleInfoData> updateBattleInfoData = null;
        
        public override void InitData()
        {
            gold = 10000;
            towerSummonDefaultLevel = 1;
        }

        public override void ResetData()
        {
            gold = 0;
            towerSummonDefaultLevel = 1;
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
    }
}