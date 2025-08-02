using System;
using System.Collections;
using Melon;
using UnityEngine;

namespace GridHero
{
    public class UIBattleResult : GameElement
    {
        public GameObject uiWin;
        public UIGameLabel uiWinRetryLabel;
        public UIGameButton uiWinRetryButton;
        
        public GameObject uiLose;
        public UIGameLabel uiLoseRetryLabel;
        public UIGameButton uiLoseRetryButton;

        public Coroutine labelCoroutine = null;
        
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

            if (labelCoroutine != null)
                StopCoroutine(labelCoroutine);

            labelCoroutine = StartCoroutine(CoTouchToRestartScale(inIsWin));
        }
        
        private IEnumerator CoTouchToRestartScale(bool inIsWin)
        {
            uiWinRetryLabel.transform.localScale = Vector3.one;
            uiLoseRetryLabel.transform.localScale = Vector3.one;
            while (true)
            {
                float t = Mathf.PingPong(Time.time * 2f, 1f); // 0~1 반복
                float eased = Mathf.SmoothStep(1f, 1.1f, t);             // 커브에 따라 조절된 값
                
                if (inIsWin)
                    uiWinRetryLabel.transform.localScale = Vector3.one * eased;
                else
                    uiLoseRetryLabel.transform.localScale = Vector3.one * eased;

                yield return null;
            }
        }
    }
}