using System.Collections;

namespace GridHero.Battle
{
    public class ExecuteSkillState : BattleState
    {
        public override IEnumerator Co_StateRoutine()
        {
            yield return Co_BattleStateStartRoutine();
            
            var currSkill = battleContent.CurrTurnSkill;
            if (currSkill != null)
            {
                battleContent.UseSkill(currSkill);
                yield return currSkill.Co_SkillExecuteRoutine();
                currSkill.UpdateAllNormalTile();
            }
                
            battleStateManager?.ChangeBattleState(BATTLE_STATE.TURN_END);
        }
    }
}