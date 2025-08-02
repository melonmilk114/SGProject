
using System.Collections;

namespace GridHero.Battle
{
    public class DetermineTurnState : BattleState
    {
        public override IEnumerator Co_StateRoutine()
        {
            yield return Co_BattleStateStartRoutine();
            
            // AP가 남은 애들중 INITIATIVE가 높은 순으로 첫번째
            var turnUnit = battleContent.FindNowTunUnit();

            if (turnUnit == null)
            {
                battleStateManager?.ChangeBattleState(BATTLE_STATE.ROUND_END);
            }
            else
            {
                battleContent.CurrTurnUnit = turnUnit;
                battleStateManager?.ChangeBattleState(BATTLE_STATE.TURN_START);
            }
        }
    }
}