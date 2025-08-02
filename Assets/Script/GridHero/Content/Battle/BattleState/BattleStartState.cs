using System.Collections;

namespace GridHero.Battle
{
    public class BattleStartState : BattleState
    {
        public override IEnumerator Co_StateRoutine()
        {
            yield return Co_BattleStateStartRoutine();

            battleStateManager?.ChangeBattleState(BATTLE_STATE.ROUND_START);
        }
    }
}