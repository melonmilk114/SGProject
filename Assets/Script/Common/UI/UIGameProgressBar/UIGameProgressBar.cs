using System;
using UnityEngine;
using UnityEngine.UI;

namespace Melon
{
    public class UIGameProgressBar : MonoBehaviour
    {
        public enum ProgressBarMode
        {
            LeftToRight,
            RightToLeft,
            TopToBottom,
            BottomToTop
        }

        [HideInInspector] public ProgressBarMode progressBarMode = ProgressBarMode.LeftToRight;
        private float _progressValue = 0.5f; // 0~1 사이 값 (예시)
        public float progressValue
        {
            get { return _progressValue; }
            set
            {
                _progressValue = Mathf.Clamp01(value); // 0~1 사이로 제한
                OnValueChanged(); // 값이 변경되면 호출
            }
        } // 0~1 사이 값 (예시)

        public UIGameImage backImage;
        public UIGameImage frontImage;
        
#if UNITY_EDITOR
        protected void Reset()
        {
            Init_Editor();
        }
#endif

        public void Init_Editor()
        {
            var findList = CommonUtils.FindChildObjects<UIGameImage>(gameObject);
            backImage = findList.Find(inFind => inFind.name == "back");
            if (backImage == null)
            {
                var backImageObj = new GameObject("back");
                backImageObj.transform.SetParent(transform, false);
                backImage = backImageObj.AddComponent<UIGameImage>();
                    
                RectTransform rect = backImage.GetComponent<RectTransform>();

                rect.anchorMin = new Vector2(0, 0);
                rect.anchorMax = new Vector2(1, 1);
                rect.offsetMin = Vector2.zero;  // Left/Bottom
                rect.offsetMax = Vector2.zero;  // Right/Top
                
                backImage.transform.SetAsLastSibling();
            }

            findList = CommonUtils.FindChildObjects<UIGameImage>(gameObject);
            frontImage = findList.Find(inFind => inFind.name == "front");
            if (frontImage == null)
            {
                var frontImageObj = new GameObject("front");
                frontImageObj.transform.SetParent(transform, false);
                frontImage = frontImageObj.AddComponent<UIGameImage>();
                frontImage.color = Color.red;
                OnProgressBarModeChanged();
                OnValueChanged();
                    
                RectTransform rect = frontImage.GetComponent<RectTransform>();

                rect.anchorMin = new Vector2(0, 0);
                rect.anchorMax = new Vector2(1, 1);
                rect.offsetMin = Vector2.zero;  // Left/Bottom
                rect.offsetMax = Vector2.zero;  // Right/Top
                
                backImage.transform.SetAsFirstSibling();
            }
        }
        
        public void OnProgressBarModeChanged()
        {
            if (frontImage?.sprite != null)
            {
                frontImage.type = Image.Type.Filled;
                
                switch (progressBarMode)
                {
                    case ProgressBarMode.LeftToRight:
                        frontImage.fillMethod = Image.FillMethod.Horizontal;
                        frontImage.fillOrigin = 0;
                        break;
                    case ProgressBarMode.RightToLeft:
                        frontImage.fillMethod = Image.FillMethod.Horizontal;
                        frontImage.fillOrigin = 1;
                        break;
                    case ProgressBarMode.TopToBottom:
                        frontImage.fillMethod = Image.FillMethod.Vertical;
                        frontImage.fillOrigin = 1;
                        break;
                    case ProgressBarMode.BottomToTop:
                        frontImage.fillMethod = Image.FillMethod.Vertical;
                        frontImage.fillOrigin = 0;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            
            
            // 선택이 바뀔 때 실행할 코드
            Debug.Log("ProgressBarMode가 변경됨: " + progressBarMode);
            // 원하는 동작 추가
        }

        public void OnValueChanged()
        {
            frontImage.fillAmount = _progressValue;
        }
    }
}