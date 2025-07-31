using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Melon;

namespace GridHeroes.Battle
{
    public class MineralAttackSkill : UnitSkill 
    {
        public override List<TileObject> FindTargetAvailableTiles()
        {
            var returnValue = new List<TileObject>();
            var findUnit = battleContent.GetAdjacentUnits(caster, caster.GetEnemyFaction());
            var casterTile = battleContent.FindTileByCoord(caster.tileOffsetCoord);
            // 캐스터의 좌우측인지 확인
            findUnit.ForEach(inForItem =>
            {
                if(inForItem.tileOffsetCoord == battleContent.GetLeftTile(casterTile)?.offsetCoord ||
                   inForItem.tileOffsetCoord == battleContent.GetRightTile(casterTile)?.offsetCoord)
                    returnValue.Add(battleContent.GetTileByUnit(inForItem));
            });

            return returnValue;
        }

        public override List<TileObject> FindEffectTiles()
        {
            var returnValue = new List<TileObject>();
            if (targetSelectTile == null)
                return returnValue;

            returnValue.Add(targetSelectTile);
            if (caster.tileOffsetCoord.x > targetSelectTile.offsetCoord.x)
            {
                // 왼쪽으로 두칸
                returnValue.Add(battleContent.GetLeftTile(targetSelectTile));
            }
            else
            {
                // 오른쪽으로 두칸
                returnValue.Add(battleContent.GetRightTile(targetSelectTile));
            }

            return returnValue;
        }

        public override IEnumerator Co_SkillExecuteRoutine()
        {
            caster.ChangeFacingDirection(targetSelectTile.transform.position);
            yield return caster.PlayAnimation(UNIT_ANIM_TYPE.SKILL_1);

            // 맞은 캐릭터 처리
            var units = battleContent.FindUnitByTile(effectTiles);
            var takeDamageRoutines = new List<IEnumerator>();
            int damage = caster.GetStatValue(STAT_TYPE.ATK);
            units.ForEach(inForItem =>
            {
                takeDamageRoutines.Add(inForItem.TakeDamage(damage));
            });
            
            yield return this.WhenAll(takeDamageRoutines.ToArray());
            
            caster.PlayIdleAnimation();
        }
    }
}