using System;
using System.Collections;
using UnityEngine;

namespace Melon
{
    public class UIAnimationScale : UIAnimation
    {
        public Vector3 startScale = new Vector3(1f, 1f, 1f);
        public Vector3 endScale = new Vector3(1f, 1f, 1f);
        private RectTransform rect;

        protected bool isDownScale = true;
        
        protected virtual void Awake()
        {
            rect = GetComponent<RectTransform>();
        }
        
        void OnDisable()
        {
            //SetLocalScale(endScale);
        }
        
        private void SetLocalScale(Vector3 inScale)
        {
            rect.localScale = inScale;
        }
        
        public override void PlayUIAnimation(Action inEndAction = null)
        {
            if (aniCoroutine != null)
            {
                StopCoroutine(aniCoroutine);
                SetLocalScale(endScale);
            }
            aniCoroutine = StartCoroutine(CoPlayUIAnimation(inEndAction));
        }

        private IEnumerator CoPlayUIAnimation(Action inEndAction = null)
        {
            float time = 0f;
            while (time < duration)
            {
                float t = time / duration;
                float curveT = scaleCurve.Evaluate(t);
                transform.localScale = Vector3.LerpUnclamped(startScale, endScale, curveT);
                time += Time.unscaledDeltaTime;
                yield return null;
            }
            
            transform.localScale = endScale;
            
            inEndAction?.Invoke();
        }
    }
}