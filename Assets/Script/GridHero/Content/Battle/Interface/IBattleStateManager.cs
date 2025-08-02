using System;
using System.Collections;

namespace GridHero.Battle
{
    public interface IBattleStateManager
    {
        public void ChangeBattleState(BATTLE_STATE inState);
        public void AddBattleStateDelegate(BATTLE_STATE inState, Func<IEnumerator> inDelegate);
        public void RemoveBattleStateDelegate(BATTLE_STATE inState, Func<IEnumerator> inDelegate);
        public void ClearBattleStateDelegate(BATTLE_STATE inState);
    }
}