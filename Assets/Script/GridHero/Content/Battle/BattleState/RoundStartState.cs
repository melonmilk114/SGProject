using System.Collections;

namespace GridHero.Battle
{
    public class RoundStartState : BattleState
    {
        public override IEnumerator Co_StateRoutine()
        {
            yield return Co_BattleStateStartRoutine();
            
            battleContent?.ClearCurrTurn();
            
            battleStateManager?.ChangeBattleState(BATTLE_STATE.DETERMINE_TURN);
        }
    }
}