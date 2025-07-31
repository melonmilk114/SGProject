namespace LuckyDefense.Interface
{
    // 타워 선택 메뉴 인터페이스
    public interface ITowerSelectMenu
    {
        public bool IsTowerUpgradeAvailable(TowerGroupObject inTowerGroup);
        public void TowerUpgrade(TowerGroupObject inTowerGroup);
        public void TowerRemove(TowerGroupObject inTowerGroup);
    }
}