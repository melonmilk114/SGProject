using TMPro;
using UnityEditor;
using UnityEngine;

namespace Melon
{
    public class UIGameLabel : TextMeshProUGUI
    {
#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();
            Init_Editor();
        }
#endif
        public void Init_Editor()
        {
            var fontAsset = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>("Assets/Resources/Font/NanumBarunGothic SDF.asset");
            if (fontAsset != null)
            {
                font = fontAsset;
                EditorUtility.SetDirty(this);
            }
            else
            {
                Debug.LogWarning("Font Asset을 찾을 수 없습니다. 경로를 확인하세요.");
            }
            
            
            text = "TEXT";
            color = Color.black;
            enableAutoSizing = true;
            fontSizeMin = 10;
            fontSizeMax = 100;
                
            alignment = TextAlignmentOptions.Center;
        }

        public void SetText(string inStr)
        {
            text = inStr;
        }
    }
}