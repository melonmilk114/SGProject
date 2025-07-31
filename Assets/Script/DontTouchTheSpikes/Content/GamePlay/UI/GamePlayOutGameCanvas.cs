using Melon;
using UnityEngine;

namespace DontTouchTheSpikes.UI
{
    public class GamePlayOutGameCanvas : GameElement
    {
        public IGamePlayContent gamePlayContent;
        
        public GameObject uiGameReadyObj;
        public UIGameLabelButton uiGameStartButton;
        
        public GameObject uiGameOverObj;
        public UIGameLabelButton uiGameReadyButton;
        public UIGameLabelButton uiGameRetryButton;

        public override void OnAwakeFunc()
        {
            base.OnAwakeFunc();
            
            uiGameStartButton.SetClickAction(() =>
            {
                gamePlayContent?.GameStart();
            });
            
            uiGameReadyButton.SetClickAction(() =>
            {
                gamePlayContent?.GameReady();
            });
            
            uiGameRetryButton.SetClickAction(() =>
            {
                gamePlayContent?.GameReady();
            });
        }

        public void UpdateGameState()
        {
            uiGameReadyObj.gameObject.SetActive(false);
            uiGameOverObj.gameObject.SetActive(false);
            
            switch (gamePlayContent.GetGameState())
            {
                case GAME_STATE.READY:
                    uiGameReadyObj.gameObject.SetActive(true);
                    break;
                case GAME_STATE.OVER:
                    uiGameOverObj.gameObject.SetActive(true);
                    break;
            }
        }
    }
}