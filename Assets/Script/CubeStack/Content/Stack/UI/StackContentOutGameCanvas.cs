using System;
using System.Collections;
using Melon;
using UnityEngine;

namespace CubeStack.UI
{
    public class StackContentOutGameCanvas : GameElement
    {
        public IStackContent stackContent;
        
        public GameObject uiGameReadyObj;
        public UIGameLabel uiTouchToStartLabel;
        public UIGameButton uiGameStartButton;
        
        public GameObject uiGamePlayObj;
        public UIGameLabel uiScoreLabel;
        
        public GameObject uiGameOverObj;
        public UIGameLabel uiTouchToReStartLabel;
        public UIGameButton uiGameReadyButton;

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
            UpdateScore(0);
        }

        public void GameOver()
        {
            UpdateGameState();
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
            StartCoroutine(CoScoreLabelPop());
        }
        
        private IEnumerator CoScoreLabelPop()
        {
            uiScoreLabel.transform.localScale = Vector3.one;
            float t = 0f;
            float duration = 0.2f;
            float scaleUp = 1.2f;
            while (t < duration)
            {
                float normalized = t / duration;
                float phase = Mathf.PingPong(normalized * 2f, 1f); // 0~1~0

                float scale = Mathf.Lerp(1f, scaleUp, phase);
                uiScoreLabel.transform.localScale = Vector3.one * scale;

                t += Time.deltaTime;
                yield return null;
            }

            uiScoreLabel.transform.localScale = Vector3.one;
        }
    }
}