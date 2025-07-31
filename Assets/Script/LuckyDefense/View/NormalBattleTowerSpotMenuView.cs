using System;
using Melon;
using UnityEngine;

namespace LuckyDefense
{
    public class NormalBattleTowerSpotMenuView : GameElement
    {
        public class ShowData
        {
            public TowerSpotObject towerSpotObject;
        }
        
        public UIGameButton uiTowerUpgradeButton;
        public UIGameImage uiTowerAttackRange;
        public Action onTowerUpgrade;
        
        public TowerSpotObject towerSpotObject;

        public override void DoPreShow(object inData = null, ActionResult inActionResult = null)
        {
            if (inData is ShowData showData)
            {
                towerSpotObject = showData.towerSpotObject;
                
                // float scale = towerSpotObject.towerData.attackRange;
                // uiTowerAttackRange.transform.localScale = new Vector3(scale, scale);
            }
            else
            {
                towerSpotObject = null;
            }
            
            base.DoPreShow(inData, inActionResult);
        }
        
        public override void DoShowCheck(object inData = null, ActionResult inActionResult = null)
        {
            if (inData is ShowData showData)
            {
                if (showData.towerSpotObject != null)
                {
                    inActionResult?.OnSuccess();    
                }
                else
                {
                    inActionResult?.OnFail("showData.towerSpotObject == null");
                }
            }
            else
            {
                inActionResult?.OnFail("showData == null");
            }
        }

        
        
    }
}