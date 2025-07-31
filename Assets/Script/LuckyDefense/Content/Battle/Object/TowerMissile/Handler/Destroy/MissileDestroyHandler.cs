using Melon;

namespace LuckyDefense
{
    public class MissileDestroyHandler : MissileHandler
    {
        public void DestroyMissile(ActionResult inResult)
        {
            inResult.OnSuccess();
        }
    }
}