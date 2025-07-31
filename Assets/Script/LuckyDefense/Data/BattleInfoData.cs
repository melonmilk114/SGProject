using System.Collections.Generic;

namespace LuckyDefense
{
#if UNITY_EDITOR
    [System.Serializable]
#endif
    public class BattleInfoData : Melon.Data
    {
        public long gold = 0;
        public long towerSummonLevel = 0;
        public Dictionary<long, long> towerStatusLevel = new Dictionary<long, long>();
        
        public DataUpdateFunc<BattleInfoData> updateBattleInfoData = null;
        
        public override void InitData()
        {
            gold = 10000;
            towerSummonLevel = 1;
        }

        public override void ResetData()
        {
            gold = 0;
            towerSummonLevel = 1;
        }

        public void PlusGold(long inGold)
        {
            gold += inGold;
            updateBattleInfoData?.UpdateData(this);
        }
        public void MinusGold(long inGold)
        {
            gold -= inGold;
            if (gold < 0)
                gold = 0;
            
            updateBattleInfoData?.UpdateData(this);
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