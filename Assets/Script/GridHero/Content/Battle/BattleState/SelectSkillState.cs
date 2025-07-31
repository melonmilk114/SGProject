using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace GridHeroes.Battle
{
    public class SelectSkillState : BattleState
    {
        public override IEnumerator Co_StateRoutine()
        {
            yield return Co_BattleStateStartRoutine();
            
            var turnUnit = battleContent.CurrTurnUnit;
            
            // 스킬 선택을 대기 해야함
            var canSkills = battleContent.GetCanUseSkills(turnUnit);
            if (turnUnit == null || canSkills.Count <= 0)
            {
                battleStateManager?.ChangeBattleState(BATTLE_STATE.TURN_END);
                yield break;
            }

            if (turnUnit.IsAutoSkillUse())
            {
                // 이동 스킬을 제외한 다른 스킬을 사용 할 수 있는 상태 라면 그걸 사용 한다
                UnitSkill useSkill = null;
                foreach (SKILL_TYPE type in Enum.GetValues(typeof(SKILL_TYPE)))
                {
                    if(type == SKILL_TYPE.NONE)
                        continue;
                    
                    var canUseSkills = battleContent.GetCanUseSkills(turnUnit, type);
                    var canUseSkill = canUseSkills.FirstOrDefault(item => item.FindTargetAvailableTiles().Count > 0);
                    if (canUseSkill != null)
                    {
                        useSkill = canUseSkill;
                        break;
                    }
                }

                if (useSkill == null)
                {
                    battleStateManager?.ChangeBattleState(BATTLE_STATE.TURN_END);
                    yield break;
                }
                
                yield return useSkill.Co_SkillSelectRoutine();
            
                battleContent.CurrTurnSkill = useSkill;
            }
            else
            {
                bool isTurnSkip = false;
                battleContent.ShowActionPanel(battleContent.CurrTurnUnit, (inUnitSkill) =>
                {
                    if (inUnitSkill != null)
                    {
                        if(inUnitSkill.FindTargetAvailableTiles().Count > 0)
                            battleContent.CurrTurnSkill = inUnitSkill;
                    }
                    else
                        isTurnSkip = true;
                });
                // 유저가 스킬을 선택 할 때까지 대기
                yield return new WaitUntil(() => isTurnSkip || battleContent.CurrTurnSkill != null);

                if (isTurnSkip)
                {
                    battleStateManager?.ChangeBattleState(BATTLE_STATE.TURN_END);
                    yield break;
                }
                yield return battleContent.CurrTurnSkill.Co_SkillSelectRoutine();
            }

            battleStateManager?.ChangeBattleState(BATTLE_STATE.EXECUTE_SKILL);
        }
    }
}