using System;
using UnityEngine;

namespace Melon
{
    public abstract class UIAnimation : MonoBehaviour
    {
        public UIAnimationCurvePresets.PresetType animationCurvePreset = UIAnimationCurvePresets.PresetType.Linear;
        protected AnimationCurve scaleCurve = UIAnimationCurvePresets.Linear;
        
        public float duration = 0.1f;
        
        protected Coroutine aniCoroutine = null;
        
        public abstract void PlayUIAnimation(Action inEndAction = null); 
        
        private void Awake()
        {
            scaleCurve = UIAnimationCurvePresets.GetCurve(animationCurvePreset);
        }
        
        void OnDisable()
        {
            if(aniCoroutine != null)
                StopCoroutine(aniCoroutine);
        }
    }
}