using System;
using Melon;

namespace DontTouchTheSpikes
{
    public class ContentManager : Melon.ContentManager
    {
        public enum ContentType
        {
            NONE,
            LOADING,
            TITLE,
            GAMEPLAY,
        }
        
        public override void InitManager()
        {
            base.InitManager();
            
            TargetObjectReceiver.DoTargetObjectInject(this, rootObj);
        }

        public override void DoShowContent(Enum inEnum, object inData = null, ActionResult inAction = null)
        {
            base.DoShowContent(inEnum, inData, inAction);
        }
    }
}