using System.Collections;

namespace GridHero.Battle
{
    public class DotDealStatusEffect : UnitStatusEffect
    {
        public override IEnumerator Co_RoundEndRoutine(UnitObject inUnit)
        {
            yield return inUnit.TakeDamage(statusEffectData.value);
            
            duration--;
            if (duration <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}