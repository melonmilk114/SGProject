using System.Collections;

namespace GridHero.Battle
{
    public class LoseEffectState : BattleEffectState
    {
        public override IEnumerator Co_StateRoutine()
        {
            DebugLogHelper.Log("LOSE EFFECT STATE");
            battleContent.ShowBattleResult(false);
            yield break;
        }
    }
}