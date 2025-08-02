using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GridHero.Battle
{
    [System.Serializable]
    public class UnitAnimMapping
    {
        public UNIT_ANIM_TYPE unitAnimType;
        public string clipName;
    }
    public class UnitAnimator : MonoBehaviour
    {
        [HideInInspector] private Animator _animator;
        
        public List<UnitAnimMapping> animMappings = new List<UnitAnimMapping>()
        {
            new UnitAnimMapping(){unitAnimType = UNIT_ANIM_TYPE.IDLE,clipName = "idle"},
            new UnitAnimMapping(){unitAnimType = UNIT_ANIM_TYPE.HIT,clipName = "take_hit"},
            new UnitAnimMapping(){unitAnimType = UNIT_ANIM_TYPE.DIE,clipName = "death"},
            new UnitAnimMapping(){unitAnimType = UNIT_ANIM_TYPE.MOVE},
            new UnitAnimMapping(){unitAnimType = UNIT_ANIM_TYPE.ATTACK},
            new UnitAnimMapping(){unitAnimType = UNIT_ANIM_TYPE.SKILL_1},
            new UnitAnimMapping(){unitAnimType = UNIT_ANIM_TYPE.SKILL_2},
        };
        
        private Dictionary<UNIT_ANIM_TYPE, float> _animDuration = new Dictionary<UNIT_ANIM_TYPE, float>();
        private Dictionary<UNIT_ANIM_TYPE, int> _animIDs = new Dictionary<UNIT_ANIM_TYPE, int>();
        
        private void Awake()
        {
            _animator = GetComponent<Animator>();
            
            var animationClips = _animator.runtimeAnimatorController.animationClips;
            foreach (var clip in animationClips)
            {
                foreach (var mapping in animMappings)
                {
                    if (mapping.clipName == clip.name)
                    {
                        _animDuration.Add(mapping.unitAnimType,clip.length);
                        
                        int id = Animator.StringToHash(clip.name);
                        _animIDs.Add(mapping.unitAnimType, id);
                    }
                }
            }
        }
        
        public IEnumerator Play(UNIT_ANIM_TYPE inUnitAnimType)
        {
            _animator.enabled = true;
            yield return null;
            
            int animId = _animIDs[inUnitAnimType];
            float animDuration = _animDuration[inUnitAnimType];
            _animator.Play(animId);
            yield return new WaitForSeconds(animDuration);
            
            _animator.enabled = false;
            yield return null;
        }

        public void PlayLoop(UNIT_ANIM_TYPE inUnitAnimType)
        {
            _animator.enabled = true;
            int animId = _animIDs[inUnitAnimType];
            _animator.Play(animId);
        }

        public void PlayIdle()
        {
            PlayLoop(UNIT_ANIM_TYPE.IDLE);
        }
    }
}