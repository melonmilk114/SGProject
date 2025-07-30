using UnityEngine;
using UnityEngine.UI;

namespace Melon
{
    public class UIGameImage : Image
    {
#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();
            Init_Editor();
        }
#endif
        public void Init_Editor()
        {
            // 기본 설정
            if (sprite == null)
            {
                sprite = Resources.Load<Sprite>("Sprite/square");
            }
        }
    }
}