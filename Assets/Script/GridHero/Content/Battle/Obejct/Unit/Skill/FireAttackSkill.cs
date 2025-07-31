using System.Collections;
using System.Collections.Generic;

namespace GridHeroes.Battle
{
    public class FireAttackSkill : MeleeAttackSkill
    {
        public DotDealStatusEffect statusEffect;
        
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