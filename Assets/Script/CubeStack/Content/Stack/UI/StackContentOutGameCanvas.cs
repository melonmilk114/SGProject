using System;
using Melon;
using UnityEngine;

namespace CubeStack.UI
{
    public class StackContentOutGameCanvas : GameElement
    {
        public IStackContent stackContent;
        
        public GameObject uiGameReadyObj;
        public UIGameLabelButton uiGameStartButton;
        
        public GameObject uiGamePlayObj;
        public UIGameLabel uiScoreLabel;
        
        public GameObject uiGameOverObj;
        public UIGameLabelButton uiGameReadyButton;

        public override void OnAwakeFunc()
        {
            base.OnAwakeFunc();
            
            uiGameStartButton.SetClickAction(() =>
            {
                stackContent?.GameStart();
            });
            
            uiGameReadyButton.SetClickAction(() =>
            {
                stackContent?.GameReady();
            });
        }

        public void UpdateGameState()
        {
            uiGameReadyObj.gameObject.SetActive(false);
            uiGamePlayObj.gameObject.SetActive(false);
            uiGameOverObj.gameObject.SetActive(false);
            
            switch (stackContent.GetGameState())
            {
                case GAME_STATE.READY:
                    uiGameReadyObj.gameObject.SetActive(true);
                    break;
                case GAME_STATE.PLAYING:
                    uiGamePlayObj.gameObject.SetActive(true);
                    break;
                case GAME_STATE.GAMEOVER:
                    uiGameOverObj.gameObject.SetActive(true);
                    break;
            }
        }

        public void UpdateScore(int inScore)
        {
            uiScoreLabel.SetText(inScore.ToString());
        }
    }
}