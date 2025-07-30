using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

namespace Melon.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(UIGameImage))]
    public class UIGameImageEditor : ImageEditor
    {
        protected override void OnEnable()
        {
            base.OnEnable();
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