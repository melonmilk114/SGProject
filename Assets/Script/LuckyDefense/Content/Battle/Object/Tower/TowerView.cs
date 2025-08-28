using System.Collections.Generic;
using Melon;
using UnityEngine;

namespace LuckyDefense
{
    public class TowerView : GameElement
    {
        public Animator animator;
        private Dictionary<string, int> _animIDs = new Dictionary<string, int>();

        public void PlayAnimation(Vector2 inDir)
        {
            if (animator == null || _animIDs.Count == 0)
                return;
            
            string playAnimName = "up";
            if(inDir == Vector2.up)
                playAnimName = "up";
            else if(inDir == Vector2.down)
                playAnimName = "down";
            else if(inDir == Vector2.right)
                playAnimName = "right";
            else if(inDir == Vector2.left)
                playAnimName = "left";
            
            if(_animIDs.TryGetValue(playAnimName, out int id))
                animator.Play(id);
        }
        
        public void Init(TowerTableDataItem inData)
        {
            animator.runtimeAnimatorController = ResourcesManager.Instance.GetAnimatorController(inData.animator);
            
            var animationClips = animator.runtimeAnimatorController.animationClips;
            foreach (var clip in animationClips)
            {
                int id = Animator.StringToHash(clip.name);
                _animIDs.Add(clip.name, id);
            }
        }
    }
}