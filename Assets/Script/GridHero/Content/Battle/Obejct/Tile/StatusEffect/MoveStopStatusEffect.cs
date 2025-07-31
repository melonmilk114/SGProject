using System.Collections;

namespace GridHeroes.Battle
{
    public class MoveStopStatusEffect : TileStatusEffect
    {
        public override IEnumerator Co_ApplyStatusEffect(UnitObject inUnit)
        {
            yield return base.Co_ApplyStatusEffect(inUnit);
            
            // 이동 스킬을 초기화 한다
            // MEMO : IBattleContent를 통해서 유닛의 이동 스킬을 초기화 한다
            var skills = inUnit.battleContent.GetSkills(inUnit);
            var findSkill = skills.Find(skill => skill.skillData.skillType == SKILL_TYPE.MOVEMENT);
            findSkill.ResetSkill();
            
            yield return null;
        }
    }
}