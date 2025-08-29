using System;
using Melon;

namespace LuckyDefense
{
    public class BattleContentOutGameCanvas : GameElement
    , IObserver<BattleInfoData>
    {
        public UIGameButton uiTowerBuildButton;
        public UIGameLabel uiGold;
        public UIGameLabel uiEnemyCount;

        public UIGameElement uiGameConditionPopup;
        public UIGameLabel uiGameConditionLabel;
        public UIGameLabelButton uiGameExitButton;

        public ITowerCreate towerCreate = null;
        public IBattleOutGameMenu battleOutGameMenu;
        
        public override void OnAwakeFunc()
        {
            base.OnAwakeFunc();
            
            uiTowerBuildButton?.SetClickAction(() =>
            {
                towerCreate?.CreateRandomTower();
            });
        }
        
        public void InitCanvas(ITowerCreate inTowerCreate, IBattleOutGameMenu inBattleOutGameMenu)
        {
            towerCreate = inTowerCreate;
            battleOutGameMenu = inBattleOutGameMenu;
        }
        
        public void ShowGameConditionPopup(string inMessage)
        {
            uiGameConditionLabel.SetText(inMessage);
            uiGameConditionPopup.DoShowUI();
            uiGameExitButton.SetClickAction(() =>
            {
                uiGameConditionPopup.DoHideUI();
                battleOutGameMenu?.OnClickGameExit();
            });
        }

        public void HideGameConditionPopup()
        {
            uiGameConditionPopup.DoHideUI();
        }

        #region IDataUpdateFunc

        public void OnNotify(BattleInfoData inData_1)
        {
            uiGold.SetText(inData_1.gold.ToString());
            uiEnemyCount.SetText($"{inData_1.nowMonsterCount} / {inData_1.maxMonsterCount}");
        }

        #endregion
        
    }
}