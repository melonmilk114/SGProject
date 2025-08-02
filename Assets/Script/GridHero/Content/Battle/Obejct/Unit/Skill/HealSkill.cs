using System.Collections;
using System.Collections.Generic;

namespace GridHero.Battle
{
    public class HealSkill : UnitSkill
    {
        public override List<TileObject> FindTargetAvailableTiles()
        {
            var findUnits = battleContent.GetAdjacentUnits(caster, caster.unitFaction, 3);
            List<TileObject> returnValue = new List<TileObject>();
            findUnits.ForEach(inForItem_1 =>
            {
                var hp = inForItem_1.GetHp();
                if (hp.Key >= hp.Value)
                    return;
                
                var tile = battleContent.FindTileByCoord(inForItem_1.tileOffsetCoord);
                if (tile != null)
                {
                    returnValue.Add(tile);
                }
            });

            return returnValue;
        }

        public override IEnumerator Co_SkillExecuteRoutine()
        {
            caster.ChangeFacingDirection(targetSelectTile.transform.position);
            yield return caster.PlayAnimation(UNIT_ANIM_TYPE.SKILL_1);
            
            var unit = battleContent.FindUnitByCoord(targetSelectTile.offsetCoord);
            if (unit != null)
            {
                int heal = caster.GetStatValue(STAT_TYPE.ATK);
                yield return unit.TakeHeal(heal);
            }
            
            caster.PlayIdleAnimation();
        }
    }
}