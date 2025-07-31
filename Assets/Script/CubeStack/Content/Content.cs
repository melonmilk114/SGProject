using Melon;

namespace CubeStack
{
    public class Content : Melon.Content
        , ITargetObjectReceiver<ContentManager>
    {
        #region ITargetObjectReceiver<ContentManager>
        ContentManager ITargetObjectReceiver<ContentManager>.GetTargetObject { get; set; }
        public void SetTargetObject(ContentManager inObject)
        {
            ((ITargetObjectReceiver<ContentManager>) this).GetTargetObject = inObject;
        }
        
        public ContentManager GetContentManager(System.Action<ContentManager> inOnNotNull = null)
        {
            var returnManager = ((ITargetObjectReceiver<ContentManager>) this).GetTargetObject;
            if (inOnNotNull != null)
                inOnNotNull.Invoke(returnManager);

            return returnManager;
        }
        #endregion
    }
}