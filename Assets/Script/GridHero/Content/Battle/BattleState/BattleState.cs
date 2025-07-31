using System;
using System.Collections;
using System.Linq;
using Melon;
using UnityEngine;

namespace GridHeroes.Battle
{
    public abstract class BattleState : MonoBehaviour
    {
        #region Delegate

        public event Func<IEnumerator> battleStateChangeDelegates;
        public abstract IEnumerator Co_StateRoutine();
        
        public void AddBattleStateDelegate(Func<IEnumerator> inDelegate)
        {
            battleStateChangeDelegates += inDelegate;
        }
        
        public void RemoveBattleStateDelegate(Func<IEnumerator> inDelegate)
        {
            battleStateChangeDelegates -= inDelegate;
        }
        
        public void ClearBattleStateDelegates()
        {
            if (battleStateChangeDelegates == null)
                return;
            
            foreach (var d in battleStateChangeDelegates?.GetInvocationList())
            {
                battleStateChangeDelegates -= (Func<IEnumerator>)d;
            }
        }
        
        public IEnumerator Co_BattleStateStartRoutine()
        {
            if (battleStateChangeDelegates != null)
            {
                var coroutines = battleStateChangeDelegates.GetInvocationList()
                    .Cast<Func<IEnumerator>>()
                    .Select(func => func())
                    .ToArray();

                yield return this.ConsecutiveAll(coroutines);
            }
        }

        #endregion
        
        public IBattleStateManager battleStateManager;
        public IBattleContent battleContent;
    }
}