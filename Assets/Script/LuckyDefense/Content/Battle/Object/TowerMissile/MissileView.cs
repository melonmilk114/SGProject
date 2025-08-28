using Melon;
using UnityEngine;

namespace LuckyDefense
{
    public class MissileView : GameElement
    {
        public SpriteRenderer sprite;
        public void Init(MissileTableDataItem inData)
        {
            sprite.sprite = ResourcesManager.Instance.GetSprite(inData.sprite);
        }
    }
}