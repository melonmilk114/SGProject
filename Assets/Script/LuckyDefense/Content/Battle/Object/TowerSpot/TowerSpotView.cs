using Melon;
using UnityEngine;

namespace LuckyDefense
{
    public class TowerSpotView : GameElement
    {
        public SpriteRenderer background;
        
        
        
        
        public UIGameButton uiUpgradeButton;
        public UIGameButton uiSellButton;
        public UIGameImage uiAttackRange;
        
        public void ShowAttackRange(float inRange)
        {
            uiAttackRange.transform.localScale = new Vector3(inRange, inRange);
            uiAttackRange.gameObject.SetActive(true);
        }
        
        public void HideAttackRange()
        {
            uiAttackRange.gameObject.SetActive(false);
        }


        public void ShowRedBack()
        {
            background.color = new Color(1, 0, 0, 0.4f);
        }

        public void ShowWhiteBack()
        {
            background.color = new Color(1, 1, 1, 0f);
        }
    }
}