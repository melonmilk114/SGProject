using System;
using Melon;

namespace LuckyDefense
{
    public class BattleContentOutGameCanvas : GameElement
    , IObserver<BattleInfoData>
    {
        public UIGameButton uiTowerBuildButton;
        public UIGameLabel uiGold;

        public ITowerCreate towerCreate = null;
        
        public override void OnAwakeFunc()
        {
            base.OnAwakeFunc();
            
            uiTowerBuildButton?.SetClickAction(() =>
            {
                towerCreate?.CreateRandomTower();
            });
        }
        
        public void InitCanvas(ITowerCreate inTowerCreate)
        {
            towerCreate = inTowerCreate;
        }

        #region IDataUpdateFunc

        public void OnNotify(BattleInfoData inData_1)
        {
            uiGold.SetText(inData_1.gold.ToString());
        }

        #endregion
        
    }
}