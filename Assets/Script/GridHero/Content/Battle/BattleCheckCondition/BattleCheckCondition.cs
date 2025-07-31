using UnityEngine;

namespace GridHeroes.Battle
{
    public abstract class BattleCheckCondition : ScriptableObject
    {
        public IBattleContent battleContent = null;
        
        public abstract bool CheckCondition();
    }
}