using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GridHero.Battle
{
    public class MeleeAttackSkill : UnitSkill
    {
        // public override bool IsRealUseSkill(UnitObject inCaster)
        // {
        //     caster = inCaster;
        //     var list = FindAvailableSkillTiles();
        //     return list.Count > 0;
        // }

        public override void UpdateSkillTargetScore()
        {
            battleContent.ResetSkillTargetScore();
            
            var findUnits = battleContent.GetAdjacentUnits(caster, caster.GetEnemyFaction());
            findUnits.ForEach(inForItem_1 =>
            {
                var tile = battleContent.FindTileByCoord(inForItem_1.tileOffsetCoord);
                if (tile != null)
                {
                    tile.skillTargetScore = 10;
                }
            });
        }

        public override List<TileObject> FindTargetAvailableTiles()
        {
            var findUnits = battleContent.GetAdjacentUnits(caster, caster.GetEnemyFaction());
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