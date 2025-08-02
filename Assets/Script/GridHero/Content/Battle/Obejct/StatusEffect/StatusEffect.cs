using System.Collections;
using UnityEngine;

namespace GridHero.Battle
{
    public class StatusEffect : MonoBehaviour
    {
        public StatusEffectData statusEffectData = new StatusEffectData();
        public int duration = int.MaxValue;

        public virtual IEnumerator Co_ApplyStatusEffect(UnitObject inUnit)
        {
            duration = statusEffectData.duration;
            yield return null;
        }
        
        public virtual IEnumerator Co_RemoveStatusEffect(UnitObject inUnit)
        {
            yield return null;
        }
        
        public virtual IEnumerator Co_TurnStartRoutine(UnitObject inUnit)
        {
            //DecreaseDuration();
            yield return null;
        }
        
        public virtual IEnumerator Co_TurnEndRoutine(UnitObject inUnit)
        {
            //DecreaseDuration();
            yield break;
        }
        
        public virtual IEnumerator Co_RoundStartRoutine(UnitObject inUnit)
        {
            //DecreaseDuration();
            yield break;
        }
        
        public virtual IEnumerator Co_RoundEndRoutine(UnitObject inUnit)
        {
            //DecreaseDuration();
            yield break;
        }
    }
}