using Melon;
using UnityEngine;

namespace LuckyDefense
{
    public class MonsterObjectView : GameElement
    {
        public SpriteRenderer sprite;
        
        public UIMonsterHpBar uiHpBar = null;
        
        public void Init(MonsterTableDataItem inData)
        {
            sprite.sprite = ResourcesManager.Instance.GetSprite(inData.sprite);
        }
        
        public void AttachHpBar(UIMonsterHpBar inHpBar)
        {
            uiHpBar = inHpBar;
            uiHpBar.AttachTarget(transform);
        }
        
        public void SetHp(float inHpRatio)
        {
            uiHpBar.SetHpRatio(inHpRatio);
        }

        public void Death()
        {
            uiHpBar.OnObjectDestroy();
        }
    }
}