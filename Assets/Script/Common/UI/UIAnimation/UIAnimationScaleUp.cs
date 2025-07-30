namespace Melon
{
    public class UIAnimationScaleUp : UIAnimationScale
    {
        protected override void Awake()
        {
            base.Awake();
            isDownScale = false;
        }
    }
}