using System.Collections;

namespace GridHero.Battle
{
    public class BattleEndState : BattleState
    {
        public override IEnumerator Co_StateRoutine()
        {
            yield return Co_BattleStateStartRoutine();
        }
    }
}