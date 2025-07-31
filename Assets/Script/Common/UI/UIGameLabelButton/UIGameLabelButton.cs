using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Melon
{
    public class UIGameLabelButton : UIGameButton
    {
        public UIGameLabel label;
        
              
#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();
            Init_Editor();
        }
#endif
        public override void Init_Editor()
        {
            base.Init_Editor();
            // 기본 설정
            if (label == null)
            {
                label = gameObject.GetComponentInChildren<UIGameLabel>();
                if (label == null)
                {
                    var labelObj = new GameObject("label");
                    labelObj.transform.SetParent(transform, false);
                    label = labelObj.AddComponent<UIGameLabel>();
                    
                    RectTransform rect = label.GetComponent<RectTransform>();

                    rect.anchorMin = new Vector2(0, 0);
                    rect.anchorMax = new Vector2(1, 1);
                    rect.offsetMin = Vector2.zero;  // Left/Bottom
                    rect.offsetMax = Vector2.zero;  // Right/Top
                }
            }
        }
        
        public void SetText(string inStr)
        {
            label?.SetText(inStr);
        }
    }
}