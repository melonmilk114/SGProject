using System;
using Melon;

namespace GridHeroes
{
    public class ContentManager : Melon.ContentManager
    {
        public enum ContentType
        {
            NONE,
            LOADING,
            TITLE,
            MAIN,
            BATTLE,
        }
        
        public override void InitManager()
        {
            base.InitManager();
            
            TargetObjectReceiver.DoTargetObjectInject(this, rootObj);
        }

        public override void DoShowContent(Enum inEnum, object inData = null, ActionResult inAction = null)
        {
            switch (inEnum)
            {
                case ContentType.NONE:
                    break;
                case ContentType.LOADING:
                    break;
                case ContentType.TITLE:
                    break;
                case ContentType.BATTLE:
                    break;
                default:
                    inAction?.OnFail("");
                    break;
            }
            
            base.DoShowContent(inEnum, inData, inAction);
        }
    }
}