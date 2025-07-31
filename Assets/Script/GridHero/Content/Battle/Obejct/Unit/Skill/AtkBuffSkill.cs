using System.Collections;
using System.Collections.Generic;

namespace GridHeroes.Battle
{
    public class AtkBuffSkill : UnitSkill
    {
        public UnitStatModifier statModifier = null;
        public override List<TileObject> FindTargetAvailableTiles()
        {
            var findUnits = battleContent.GetAdjacentUnits(caster, caster.unitFaction, 3);
            List<TileObject> returnValue = new List<TileObject>();
            findUnits.ForEach(inForItem_1 =>
            {
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

            // 맞은 캐릭터 처리
            var unit = battleContent.FindUnitByCoord(targetSelectTile.offsetCoord);
            if (unit != null)
            {
                if (statModifier != null)
                {
                    var newStatModifier = Instantiate(statModifier);
                    yield return unit.AddStatModifier(newStatModifier);
                }
            }
            
            caster.PlayIdleAnimation();
        }
    }
}