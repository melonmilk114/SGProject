using System.Collections;

namespace GridHeroes.Battle
{
    public class DotHealStatusEffect : UnitStatusEffect
    {
        public override IEnumerator Co_RoundEndRoutine(UnitObject inUnit)
        {
            yield return inUnit.TakeHeal(statusEffectData.value);
            duration--;
            if (duration <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}