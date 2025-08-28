using Melon;
using UnityEngine;

namespace LuckyDefense
{
    public class TowerGroupView : GameElement
    {
        public SpriteRenderer towerAtkRange;

        public void InitView(TowerTableDataItem inTowerTableDataItem)
        {
            towerAtkRange.transform.localScale = Vector3.one * inTowerTableDataItem.attack_range;
        }
        
        public void ShowAttackRange(bool inShow)
        {
            towerAtkRange.enabled = inShow;
        }
    }
}