using UnityEditor;
using UnityEngine;

namespace Melon
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(UIGameProgressBar))]
    public class UIGameProgressBarEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            UIGameProgressBar progressBar = (UIGameProgressBar)target;
            if (progressBar != null)
            {
                // 이전 값 저장
                UIGameProgressBar.ProgressBarMode prevMode = progressBar.progressBarMode;

                // EnumPopup으로 선택 메뉴 표시
                progressBar.progressBarMode = (UIGameProgressBar.ProgressBarMode)EditorGUILayout.EnumPopup("Progress Bar Mode", progressBar.progressBarMode);

                // 값이 바뀌었을 때 함수 호출
                if (prevMode != progressBar.progressBarMode)
                {
                    progressBar.OnProgressBarModeChanged(); // 함수 호출
                }
                
                // value 슬라이더 추가 (0~1 사이)
                float prevValue = progressBar.progressValue;
                progressBar.progressValue = EditorGUILayout.Slider("Value", progressBar.progressValue, 0f, 1f);

                // 값이 바뀌면 원하는 함수 호출
                if (!Mathf.Approximately(prevValue, progressBar.progressValue))
                {
                    progressBar.OnValueChanged(); // 필요하면 함수 호출
                }
            }

            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }

            
        }
    }
}