using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GridHero.Battle
{
    [System.Serializable]
    public class TileAnimMapping
    {
        public TILE_ANIM_TYPE tileAnimType;
        public string clipName;
    }
    public class TileAnimator : MonoBehaviour
    {
        [HideInInspector] private Animator _animator;
        
        public List<TileAnimMapping> animMappings = new List<TileAnimMapping>()
        {
            new TileAnimMapping(){tileAnimType = TILE_ANIM_TYPE.IDLE,clipName = "idle"},
        };
        
        private Dictionary<TILE_ANIM_TYPE, float> _animDuration = new Dictionary<TILE_ANIM_TYPE, float>();
        private Dictionary<TILE_ANIM_TYPE, int> _animIDs = new Dictionary<TILE_ANIM_TYPE, int>();
        
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
                        _animDuration.Add(mapping.tileAnimType,clip.length);
                        
                        int id = Animator.StringToHash(clip.name);
                        _animIDs.Add(mapping.tileAnimType, id);
                    }
                }
            }
        }
        
        public IEnumerator Play(TILE_ANIM_TYPE inUnitAnimType)
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

        public void PlayLoop(TILE_ANIM_TYPE inUnitAnimType)
        {
            _animator.enabled = true;
            int animId = _animIDs[inUnitAnimType];
            _animator.Play(animId);
        }

        public void PlayIdle()
        {
            PlayLoop(TILE_ANIM_TYPE.IDLE);
        }
    }
}