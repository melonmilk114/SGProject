using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Melon
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(UIGameLabelButton), true)]
    public class UIGameLabelButtonEditor : UIGameButtonEditor
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            
            // var button = target as UIGameLabelButton;
            // if (button != null)
            // {
            //     // 기본 설정
            //     if (button.label == null)
            //     {
            //         var label = button.gameObject.GetComponentInChildren<UIGameLabel>();
            //         if (label == null)
            //         {
            //             var labelObj = new GameObject("label");
            //             labelObj.transform.SetParent(button.transform, false);
            //             label = labelObj.AddComponent<UIGameLabel>();
            //             label.Init_Editor();
            //             
            //             RectTransform rect = label.GetComponent<RectTransform>();
            //
            //             rect.anchorMin = new Vector2(0, 0);
            //             rect.anchorMax = new Vector2(1, 1);
            //             rect.offsetMin = Vector2.zero;  // Left/Bottom
            //             rect.offsetMax = Vector2.zero;  // Right/Top
            //         }
            //     }
            // }
            
        }
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            UIGameLabelButton button = (UIGameLabelButton)target;
            button.label = (UIGameLabel)EditorGUILayout.ObjectField("GameLabel", button.label, typeof(UIGameLabel), true);

            // 변경 사항 적용
            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
        }
    }
}