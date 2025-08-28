using System;
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
        }
        
        public UIGameLabelButton uiTowerSellButton;
        public UIGameLabelButton uiTowerTierUpButton;

        public TowerGroupObject selectTowerGroup = null;

        public ITowerSelectMenu towerSelectMenu = null;
        
        public override void OnAwakeFunc()
        {
            base.OnAwakeFunc();

            uiTowerSellButton.SetClickAction(() =>
            {
                towerSelectMenu?.SellTowerGroup(selectTowerGroup);
            });
            uiTowerTierUpButton.SetClickAction(() =>
            {
                towerSelectMenu?.MergeTowerGroup(selectTowerGroup);
            });
        }
        
        public override void DoPreShow(object inData = null)
        {
            base.DoPreShow(inData);
            
            if (inData is ShowData showData)
            {
                selectTowerGroup = showData.selectTowerGroup;
            }
        }
        
        public override void DoPostShow(object inData = null)
        {
            base.DoPostShow(inData);

            if(towerSelectMenu.IsTowerMergeAvailable(selectTowerGroup))
                uiTowerTierUpButton.DoShow();
            else
                uiTowerTierUpButton.DoHide();
        }
    }
}