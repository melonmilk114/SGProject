using LuckyDefense.Interface;
using Melon;
using Unity.VisualScripting;
using UnityEngine;

namespace LuckyDefense
{
    public class UITowerSelectMenu : UIGameElement
    {
        public class ShowData
        {
            public TowerGroupObject selectTowerGroup;
            public ITowerSelectMenu towerSelectMenuInterface;
        }

        public UIGameLabelButton uiTowerTierUpButton;
        public UIGameLabelButton uiTowerRemoveButton;

        public TowerGroupObject selectTowerGroup = null;
        public ITowerSelectMenu towerSelectMenuInterface = null;
        
        public override void OnAwakeFunc()
        {
            base.OnAwakeFunc();
            
            uiTowerTierUpButton.SetClickAction(() =>
            {
                towerSelectMenuInterface?.TowerUpgrade(selectTowerGroup);
            });
            
            uiTowerRemoveButton.SetClickAction(() =>
            {
                towerSelectMenuInterface?.TowerRemove(selectTowerGroup);
            });

            showAnimation = CommonUtils.FindComponent<UIAnimationScaleUp>(gameObject);
            hideAnimation = CommonUtils.FindComponent<UIAnimationScaleDown>(gameObject);
        }
        
        public override void DoPreShow(object inData = null, ActionResult inActionResult = null)
        {
            if (inData is ShowData showData)
            {
                selectTowerGroup = showData.selectTowerGroup;
                towerSelectMenuInterface = showData.towerSelectMenuInterface;
            }
            
            base.DoPreShow(inData, inActionResult);
        }

        public override void DoPostShow(object inData = null, ActionResult inActionResult = null)
        {
            base.DoPostShow(inData, inActionResult);

            uiTowerTierUpButton.interactable = towerSelectMenuInterface.IsTowerUpgradeAvailable(selectTowerGroup);
        }
    }
}