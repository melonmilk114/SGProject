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
        }

        public void DoShowUI(object inData = null)
        {
            DoShow(inData);
        }
        
        public override void DoPostShow(object inData = null, ActionResult inActionResult = null)
        {
            base.DoPostShow(inData, inActionResult);
            
            showAnimation?.PlayUIAnimation();
        }

        public void DoHideUI(object inData = null)
        {
            if (gameObject.activeInHierarchy == false)
                return;
            
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