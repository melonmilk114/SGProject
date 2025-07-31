namespace LuckyDefense.Interface
{
    public interface IBattleService
    {
        public long GetTowerSummon();
        public long GetTowerSummonCost();
        public bool IsTowerSummonCostEnough();
        public void PayTowerSummonCost();
        public long GetTowerTierUpCost(long inSn);
        public bool IsTowerUpgradeCostEnough(long inSn);
        public void PayTowerTierUpCost(long inSn);

        public void RefundTowerCost(long inSn);
        public void MonsterDeathReward(long inSn);

        public long GetTowerMissileDamage(long inTowerSn);
    }
}