using Script;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

namespace Melon
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(UIGamePopup), true)]
    public class UIGamePopupEditor : UnityEditor.Editor
    {
        protected virtual void OnEnable()
        {
            var popup = target as UIGamePopup;
            if (popup != null)
            {
                if (popup.isScaleAni)
                {
                    // UIAnimationScale 으로 대체
                    // if (popup.GetComponent<UIShowAnimation>() == null)
                    //     popup.gameObject.AddComponent<UIShowAnimation>();
                }
            }
        }
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            UIGamePopup button = (UIGamePopup)target;
            button.isScaleAni = EditorGUILayout.Toggle("UseScaleAni", button.isScaleAni);

            // 변경 사항 적용
            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
        }
    }
}