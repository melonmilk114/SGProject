using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Melon
{
    public class UIGameButton : Button
    {
        private Action onClickAction = null;
        private Action<object> onClickParamAction = null;
        private object clickParamActionData = null;
        
        public bool isClickScaleAni = true;

        private UIAnimationScaleDown _downScaleAni = null;
        public UIAnimationScaleDown downScaleAni
        {
            get
            {
                if (_downScaleAni == null && isClickScaleAni)
                {
                    _downScaleAni = CommonUtils.FindComponent<UIAnimationScaleDown>(gameObject);
                    if (_downScaleAni == null)
                    {
                        _downScaleAni = gameObject.AddComponent<UIAnimationScaleDown>();
                        _downScaleAni.startScale = new Vector3(1f, 1f, 1f);
                        _downScaleAni.endScale = new Vector3(0.9f, 0.9f, 1f);
                        _downScaleAni.duration = 0.1f;
                        _downScaleAni.animationCurvePreset = UIAnimationCurvePresets.PresetType.Linear;
                    }
                }
                return _downScaleAni;
            }
        }
        private UIAnimationScaleUp _upScaleAni = null;
        public UIAnimationScaleUp upScaleAni
        {
            get
            {
                if (_upScaleAni == null && isClickScaleAni)
                {
                    _upScaleAni = CommonUtils.FindComponent<UIAnimationScaleUp>(gameObject);
                    if (_upScaleAni == null)
                    {
                        _upScaleAni = gameObject.AddComponent<UIAnimationScaleUp>();
                        _upScaleAni.startScale = new Vector3(0.9f, 0.9f, 0.9f);
                        _upScaleAni.endScale = new Vector3(1f, 1f, 1f);
                        _upScaleAni.duration = 0.1f;
                        _upScaleAni.animationCurvePreset = UIAnimationCurvePresets.PresetType.Linear;
                    }
                }
                return _upScaleAni;
            }
        }
        
        public bool isClickSound = true;
        private UIGameButtonSound _clickSound = null;
        public UIGameButtonSound clickSound
        {
            get
            {
                if (_clickSound == null && isClickSound)
                    _clickSound = GetComponent<UIGameButtonSound>();
                return _clickSound;
            }
        }
        
#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();
            Init_Editor();
        }
#endif
        public virtual void Init_Editor()
        {
            
            // 기본 설정
            if (image == null)
            {
                image = gameObject.AddComponent<Image>();
                var sprite = Resources.Load<Sprite>("Sprite/square");
                if (sprite != null)
                    image.sprite = sprite;
                else
                    image.sprite = null;
            }

            transition = Selectable.Transition.ColorTint;
            colors = new ColorBlock
            {
                normalColor = new Color(1f, 1f, 1f, 1f),
                highlightedColor = new Color(1f, 1f, 1f, 1f),
                pressedColor = new Color(1f, 1f, 1f, 1f),
                selectedColor = new Color(1f, 1f, 1f, 1f),
                disabledColor = new Color(0.8f, 0.8f, 0.8f, 0.5f),
                colorMultiplier = 1,
                fadeDuration = 0.1f
            };
            
            if (isClickScaleAni)
            {
                if (downScaleAni == null)
                {
                    var downScaleAni = gameObject.AddComponent<UIAnimationScaleDown>();
                    downScaleAni.startScale = new Vector3(1f, 1f, 1f);
                    downScaleAni.endScale = new Vector3(0.9f, 0.9f, 1f);
                    downScaleAni.duration = 0.1f;
                    downScaleAni.animationCurvePreset = UIAnimationCurvePresets.PresetType.Linear;
                }
                
                if (upScaleAni == null)
                {
                    var downScaleAni = gameObject.AddComponent<UIAnimationScaleUp>();
                    downScaleAni.startScale = new Vector3(0.9f, 0.9f, 1f);
                    downScaleAni.endScale = new Vector3(1f, 1f, 1f);
                    downScaleAni.duration = 0.1f;
                    downScaleAni.animationCurvePreset = UIAnimationCurvePresets.PresetType.Linear;
                }
            }
            else
            {
                var compList = gameObject.GetComponents<UIAnimationScale>();
                foreach (var comp in compList)
                {
                    DestroyImmediate(comp);
                }
            }
            
            if (isClickSound)
            {
                if (clickSound == null)
                    gameObject.AddComponent<UIGameButtonSound>();
            }
            else
                DestroyImmediate(gameObject.GetComponent<UIGameButtonSound>());
        }

        public void SetClickAction(Action inClickAction)
        {
            onClickAction = inClickAction;
        }
        
        public void SetClickAction(Action<object> inClickParamAction, object inClickParamActionData = null)
        {
            SetClickActionParamData(inClickParamActionData);
            onClickParamAction = inClickParamAction;
        }

        public void SetClickActionParamData(object inClickParamActionData)
        {
            clickParamActionData = inClickParamActionData;
        }
        
        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            downScaleAni?.PlayUIAnimation();
        }
        
        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            upScaleAni?.PlayUIAnimation(OnPointerClick);
        }
        
        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            clickSound?.OnPointerClick();
            //OnPointerClick();
        }

        public void OnPointerClick()
        {
            if (interactable)
            {
                onClickAction?.Invoke();
                onClickParamAction?.Invoke(clickParamActionData);
            }
        }
    }
}