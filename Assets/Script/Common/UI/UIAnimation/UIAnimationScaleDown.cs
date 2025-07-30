namespace Melon
{
    public class UIAnimationScaleDown : UIAnimationScale
    {
        protected override void Awake()
        {
            base.Awake();
            isDownScale = true;
        }
    }
}