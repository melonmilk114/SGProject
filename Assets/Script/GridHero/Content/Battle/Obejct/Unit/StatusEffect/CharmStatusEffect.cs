using System.Collections;

namespace GridHeroes.Battle
{
    public class CharmStatusEffect : UnitStatusEffect
    {
        public UNIT_FACTION tmpFaction = UNIT_FACTION.NONE;
        
        public override IEnumerator Co_ApplyStatusEffect(UnitObject inUnit)
        {
            yield return base.Co_ApplyStatusEffect(inUnit);
            tmpFaction = inUnit.unitFaction;
            inUnit.unitFaction = inUnit.GetEnemyFaction();
            yield return null;
        }
        
        public override IEnumerator Co_RemoveStatusEffect(UnitObject inUnit)
        {
            inUnit.unitFaction = tmpFaction;
            yield return null;
        }
        
        public override IEnumerator Co_RoundEndRoutine(UnitObject inUnit)
        {
            duration--;
            if (duration <= 0)
            {
                yield return Co_RemoveStatusEffect(inUnit);
                Destroy(gameObject);
            }
        }
    }
}