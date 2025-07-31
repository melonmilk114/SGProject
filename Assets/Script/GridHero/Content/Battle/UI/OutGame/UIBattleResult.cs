using System;
using Melon;
using UnityEngine;

namespace GridHeroes
{
    public class UIBattleResult : GameElement
    {
        public GameObject uiWin;
        public UIGameLabelButton uiWinRetryButton;
        
        public GameObject uiLose;
        public UIGameLabelButton uiLoseRetryButton;
        
        public Action onGameRetry;
        
        public override void OnAwakeFunc()
        {
            uiWinRetryButton.SetClickAction(() =>
            {
                onGameRetry?.Invoke();
            });
            
            uiLoseRetryButton.SetClickAction(() =>
            {
                onGameRetry?.Invoke();
            });
        }

        public void ShowBattleResult(bool inIsWin)
        {
            uiWin.SetActive(inIsWin);
            uiLose.SetActive(!inIsWin);
        }
    }
}