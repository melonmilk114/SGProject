using System;
using LuckyDefense.Interface;
using Melon;

namespace LuckyDefense
{
    public class BattleContentOutGameCanvas : GameElement
    , IDataObserver<BattleInfoData>
    {
        public UIGameButton uiTowerBuildButton;
        public UIGameLabel uiGold;

        public ITowerCreate towerCreateInterface = null;
        
        public override void OnAwakeFunc()
        {
            base.OnAwakeFunc();
            
            uiTowerBuildButton?.SetClickAction(() =>
            {
                towerCreateInterface?.CreateRandomTower();
            });
        }

        #region IDataUpdateFunc

        public void OnDataChanged(BattleInfoData inData_1)
        {
            uiGold.SetText(inData_1.gold.ToString());
        }

        #endregion
        
    }
}