using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GridHero.Battle
{
    public class RangedAttackSkill : UnitSkill
    {
        public override List<TileObject> FindTargetAvailableTiles()
        {
            var returnValue = new List<TileObject>();
            var findUnits = battleContent.GetUnits(caster.GetEnemyFaction());
            var findTiles = battleContent.GetTileByUnit(findUnits);
            var casterTile = battleContent?.FindTileByCoord(caster.tileOffsetCoord);
            foreach (var tile in findTiles)
            {
                int manhattanDist = battleContent.GetManhattanDist(casterTile, tile);
                int yAbs = Mathf.Abs(tile.offsetCoord.y - casterTile.offsetCoord.y);
                if (manhattanDist >= 3 && manhattanDist < 6 && 
                    yAbs <= manhattanDist / 3)
                {
                    returnValue.Add(tile);
                }
            }

            return returnValue;
        }

        public override IEnumerator Co_SkillExecuteRoutine()
        {
            caster.ChangeFacingDirection(targetSelectTile.transform.position);
            yield return caster.PlayAnimation(UNIT_ANIM_TYPE.ATTACK);

            // 맞은 캐릭터 처리
            var unit = battleContent.FindUnitByCoord(targetSelectTile.offsetCoord);
            if (unit != null)
            {
                int damage = caster.GetStatValue(STAT_TYPE.ATK);
                yield return unit.TakeDamage(damage);
            }
            
            caster.PlayIdleAnimation();
        }
    }
}