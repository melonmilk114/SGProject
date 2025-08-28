using UnityEngine;

namespace Melon
{
    public class UIGameElement : GameElement
    {
        protected UIAnimation showAnimation = null;
        protected UIAnimation hideAnimation = null;
        
        public override void OnAwakeFunc()
        {
            base.OnAwakeFunc();
            
            showAnimation = CommonUtils.FindComponent<UIAnimationScaleUp>(gameObject);
            hideAnimation = CommonUtils.FindComponent<UIAnimationScaleDown>(gameObject);
        }

        public void DoShowUI(object inData = null)
        {
            DoShow(inData);
        }
        
        public override void DoPostShow(object inData = null)
        {
            base.DoPostShow(inData);
            
            showAnimation?.PlayUIAnimation();
        }
        
        public override void DoPostShow(object inData = null, ActionResult inActionResult = null)
        {
            base.DoPostShow(inData, inActionResult);
            
            showAnimation?.PlayUIAnimation();
        }

        public void DoHideUI(object inData = null)
        {
            if (hideAnimation != null)
            {
                hideAnimation.PlayUIAnimation(() =>
                {
                    DoHide(inData);
                });
            }
            else
            {
                DoHide(inData);
            }
            
        }
    }
}