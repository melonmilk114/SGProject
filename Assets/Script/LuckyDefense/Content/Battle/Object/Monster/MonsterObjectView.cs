using System;
using System.Collections.Generic;
using Melon;
using UnityEngine;

namespace LuckyDefense
{
    public class MonsterObjectView : GameElement
    {
        public Animator animator;
        private Dictionary<string, int> _animIDs = new Dictionary<string, int>();
        
        public UIMonsterHpBar uiHpBar = null;

        public void PlayAnimation(Vector2 inMoveDir)
        {
            if (animator == null || _animIDs.Count == 0)
                return;
            
            string playAnimName = "up";
            if(inMoveDir == Vector2.up)
                playAnimName = "up";
            else if(inMoveDir == Vector2.down)
                playAnimName = "down";
            else if(inMoveDir == Vector2.right)
                playAnimName = "right";
            else if(inMoveDir == Vector2.left)
                playAnimName = "left";
            
            if(_animIDs.TryGetValue(playAnimName, out int id))
                animator.Play(id);
        }
        
        public void Init(MonsterTableDataItem inData)
        {
            animator.runtimeAnimatorController = ResourcesManager.Instance.GetAnimatorController(inData.animator);
            
            _animIDs.Clear();
            var animationClips = animator.runtimeAnimatorController.animationClips;
            foreach (var clip in animationClips)
            {
                int id = Animator.StringToHash(clip.name);
                _animIDs.Add(clip.name, id);
            }
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