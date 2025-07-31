using System;
using System.Collections;
using Melon;
using UnityEngine;

namespace GridHeroes
{
    public class LoadingContent : Content
    {
        public UIGameLabel loadingLabel;
        public UIGameProgressBar loadingProgressBar;
        public UIGameLabelButton startButton;
        
        public override void OnAwakeFunc()
        {
            base.OnAwakeFunc();
            loadingLabel?.SetText("Grid Heros Loading...");
            startButton?.SetText("Game Start");
            
            startButton.SetClickAction(() =>
            {
                GetContentManager(inMgr =>
                {
                    inMgr.DoShowContent(ContentManager.ContentType.MAIN);
                });
            });
        }
        
        public override Enum GetContentType()
        {
            return ContentManager.ContentType.LOADING;
        }
        
        public override void DoPreShow(object inData = null, ActionResult inAction = null)
        {
            loadingProgressBar.progressValue = 0;
            
            inAction?.onSuccess();
        }
        
        public override void DoContentStart(object inData)
        {
            StartCoroutine(LerpZeroToOne(1, (value) =>
            {
                loadingProgressBar.progressValue = value;
                loadingLabel?.SetText($"Loading... {Mathf.RoundToInt(value * 100)}%");
            }));
        }
        
        public IEnumerator LerpZeroToOne(float duration, Action<float> onValueChanged)
        {
            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);
                onValueChanged?.Invoke(t);
                yield return null;
            }
            onValueChanged?.Invoke(1f); // 마지막에 1로 보정
        }
        
    }
}