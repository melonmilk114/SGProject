using UnityEngine;

namespace Melon
{
    public static class UIAnimationCurvePresets
    {
        public enum PresetType
        {
            Linear,
            EaseInOut,
            EaseInOutBack
        }
        
        public static AnimationCurve GetCurve(PresetType type)
        {
            return type switch
            {
                PresetType.Linear => Linear,
                PresetType.EaseInOut => EaseInOut,
                PresetType.EaseInOutBack => EaseInOutBack,
                _ => Linear
            };
        }
        public static AnimationCurve Linear => AnimationCurve.Linear(0f, 0f, 1f, 1f);
        
        public static AnimationCurve EaseInOut => AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
        public static AnimationCurve EaseInOutBack => 
            new AnimationCurve(
                new Keyframe(0f, 0f, 0f, 0f),
                new Keyframe(0.25f, -0.1f),
                new Keyframe(0.75f, 1.1f),
                new Keyframe(1f, 1f, 0f, 0f)   // 복원
            );
    }
}