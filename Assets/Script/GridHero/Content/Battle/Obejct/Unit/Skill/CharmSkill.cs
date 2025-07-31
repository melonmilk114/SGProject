using System.Collections;
using System.Collections.Generic;

namespace GridHeroes.Battle
{
    public class CharmSkill : UnitSkill
    {
        public CharmStatusEffect statusEffect;
        public override List<TileObject> FindTargetAvailableTiles()
        {
            var findUnits = battleContent.GetAdjacentUnits(caster, caster.GetEnemyFaction(), 3);
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
                if (statusEffect != null)
                {
                    var newStatusEffect = Instantiate(statusEffect);
                    yield return unit.AddStatusEffect(newStatusEffect);
                }
            }
            
            caster.PlayIdleAnimation();
        }
    }
}