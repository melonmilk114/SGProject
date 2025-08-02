using System.Collections;

namespace GridHero.Battle
{
    public class WinEffectState : BattleEffectState
    {
        public override IEnumerator Co_StateRoutine()
        {
            DebugLogHelper.Log("WIN EFFECT STATE");
            battleContent.ShowBattleResult(true);
            yield break;
        }
    }
}