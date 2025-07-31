using System.Collections;

namespace GridHeroes.Battle
{
    public class BattleEndState : BattleState
    {
        public override IEnumerator Co_StateRoutine()
        {
            yield return Co_BattleStateStartRoutine();
        }
    }
}