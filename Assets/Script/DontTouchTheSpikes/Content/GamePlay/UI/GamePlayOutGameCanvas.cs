using System.Collections;
using Melon;
using UnityEngine;

namespace DontTouchTheSpikes.UI
{
    public class GamePlayOutGameCanvas : GameElement
    {
        public IGamePlayContent gamePlayContent;
        
        public GameObject uiGameReadyObj;
        public UIGameLabel uiTouchToStartLabel;
        public UIGameButton uiGameStartButton;
        
        public GameObject uiGameOverObj;
        public UIGameLabel uiTouchToReStartLabel;
        public UIGameButton uiGameRetryButton;

        public override void OnAwakeFunc()
        {
            base.OnAwakeFunc();
            
            uiGameStartButton.SetClickAction(() =>
            {
                gamePlayContent?.GameStart();
            });

            uiGameRetryButton.SetClickAction(() =>
            {
                gamePlayContent?.GameReady();
            });
        }
        
        public override void DoPostShow(object inData = null)
        {
            base.DoPostShow(inData);

            StartCoroutine(CoTouchToStartScale());
            StartCoroutine(CoTouchToReStartScale());
        }
        
        
        private IEnumerator CoTouchToStartScale()
        {
            while (true)
            {
                float t = Mathf.PingPong(Time.time * 2f, 1f); // 0~1 반복
                float eased = Mathf.SmoothStep(1f, 1.1f, t);             // 커브에 따라 조절된 값
                
                uiTouchToStartLabel.transform.localScale = Vector3.one * eased;

                yield return null;
            }
        }
        
        private IEnumerator CoTouchToReStartScale()
        {
            while (true)
            {
                float t = Mathf.PingPong(Time.time * 2f, 1f); // 0~1 반복
                float eased = Mathf.SmoothStep(1f, 1.1f, t);             // 커브에 따라 조절된 값
                
                uiTouchToReStartLabel.transform.localScale = Vector3.one * eased;

                yield return null;
            }
        }
        
        public void GameReady()
        {
            UpdateGameState();
        }

        public void GameStart()
        {
            UpdateGameState();
        }

        public void GameOver()
        {
            UpdateGameState();
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