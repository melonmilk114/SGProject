using System.Collections;
using UnityEngine;

namespace GridHero.Battle
{
    public class UnitStatModifier : MonoBehaviour
    {
        public StatModifierData statModifierData = new StatModifierData();
        public int duration = int.MaxValue;
        
        public bool isEqualStat(STAT_TYPE inStatType)
        {
            return statModifierData.statType == inStatType;
        }
        
        public void DecreaseDuration()
        {
            duration--;
            if (duration <= 0)
            {
                Destroy(gameObject);
            }
        }
        
        public virtual IEnumerator Co_ApplyStatModifier()
        {
            duration = statModifierData.duration;
            yield return null;
        }

        // public virtual IEnumerator Co_TurnStartRoutine()
        // {
        //     DecreaseDuration();
        //     yield return null;
        // }
        //
        // public virtual IEnumerator Co_TurnEndRoutine()
        // {
        //     DecreaseDuration();
        //     yield return null;
        // }
        //
        // public virtual IEnumerator Co_RoundStartRoutine()
        // {
        //     DecreaseDuration();
        //     yield return null;
        // }
        
        public virtual IEnumerator Co_RoundEndRoutine()
        {
            DecreaseDuration();
            yield return null;
        }
    }
}