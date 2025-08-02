using System.Collections;

namespace GridHero.Battle
{
    public class TurnStartState : BattleState
    {
        public override IEnumerator Co_StateRoutine()
        {
            yield return Co_BattleStateStartRoutine();
            
            battleContent.SetTurnUnit(battleContent.CurrTurnUnit);
            battleStateManager?.ChangeBattleState(BATTLE_STATE.SELECT_SKILL);
        }
    }
}