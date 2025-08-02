using System;
using System.Collections;
using Melon;
using UnityEngine;

namespace GridHero
{
    public class TitleContent: Content
    {
        public UIGameButton uiStartButton;
        public UIGameLabel uiTouchToStartLabel;
        
        public override void OnAwakeFunc()
        {
            base.OnAwakeFunc();
            uiStartButton?.SetClickAction(() =>
            {
                GetContentManager(inMgr =>
                {
                    inMgr.DoShowContent(ContentManager.ContentType.BATTLE);
                });
            });
        }
        
        public override Enum GetContentType()
        {
            return ContentManager.ContentType.TITLE;
        }
        
        public override void DoPostShow(object inData = null)
        {
            base.DoPostShow(inData);
            StartCoroutine(CoTouchToStartScale());
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
    }
}