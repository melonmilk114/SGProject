using System.Collections;

namespace GridHero.Battle
{
    public class TurnEndState : BattleState
    {
        public override IEnumerator Co_StateRoutine()
        {
            yield return Co_BattleStateStartRoutine();

            battleContent.CurrTurnUnit.isTurnEnd = true;
            battleContent.CurrTurnUnit = null;
            battleContent.CurrTurnSkill = null;
            battleContent.SetTurnUnit(null);
            battleContent.HideActionPanel();
            battleStateManager?.ChangeBattleState(BATTLE_STATE.DETERMINE_TURN);
        }
    }
}