using TMPro;
using TMPro.EditorUtilities;
using UnityEditor;
using UnityEngine;

namespace Melon.Editor
{
    // TMP_EditorPanelUI
    [CanEditMultipleObjects]
    [CustomEditor(typeof(UIGameLabel))]
    public class UIGameLabelEditor : TMP_EditorPanelUI
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            // var label = target as UIGameLabel;
            // if (label != null)
            // {
            //     var fontAsset = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>("Assets/Resources/Font/NanumBarunGothic SDF.asset");
            //     if (fontAsset != null)
            //     {
            //         label.font = fontAsset;
            //         EditorUtility.SetDirty(label);
            //     }
            //     else
            //     {
            //         Debug.LogWarning("Font Asset을 찾을 수 없습니다. 경로를 확인하세요.");
            //     }
            // }

        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            // 변경 사항 적용
            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
        }
    }
}