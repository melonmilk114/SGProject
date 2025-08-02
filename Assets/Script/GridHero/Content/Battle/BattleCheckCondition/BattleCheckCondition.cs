using UnityEngine;

namespace GridHero.Battle
{
    public abstract class BattleCheckCondition : ScriptableObject
    {
        public IBattleContent battleContent = null;
        
        public abstract bool CheckCondition();
    }
}