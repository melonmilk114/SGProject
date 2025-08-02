using System;
using System.Collections;
using System.Linq;
using Melon;
using UnityEngine;

namespace GridHero.Battle
{
    public abstract class BattleEffectState : MonoBehaviour
    {
        public abstract IEnumerator Co_StateRoutine();
        
        public IBattleStateManager battleStateManager;
        public IBattleContent battleContent;
    }
}