using System.Collections;

namespace GridHero.Battle
{
    public class RoundEndState : BattleState
    {
        public override IEnumerator Co_StateRoutine()
        {
            yield return Co_BattleStateStartRoutine();
            
            // 선택할 캐릭터가 없어서 이쪽으로 옴
            
            battleStateManager?.ChangeBattleState(BATTLE_STATE.ROUND_START);
            
        }
    }
}