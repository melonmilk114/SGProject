namespace LuckyDefense
{
    public interface ITowerSelectMenu
    {
        public void SellTowerGroup(TowerGroupObject inTowerGroup);
        public void MergeTowerGroup(TowerGroupObject inTowerGroup);
        public bool IsTowerMergeAvailable(TowerGroupObject inTowerGroup);
    }
}