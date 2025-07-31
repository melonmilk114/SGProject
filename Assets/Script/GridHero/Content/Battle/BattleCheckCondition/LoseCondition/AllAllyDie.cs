using System.Linq;
using UnityEngine;

namespace GridHeroes.Battle
{
    [CreateAssetMenu(menuName = "Lose Condition/All Ally Die", fileName = "All Ally Die", order = 0)]
    public class AllAllyDie : BattleCheckCondition
    {
        public override bool CheckCondition()
        {
            if (battleContent == null)
            {
                DebugLogHelper.LogError("BattleContent is not set for AllAllyDie condition.");
                return false;
            }
            
            var findList = battleContent.GetUnits(UNIT_FACTION.ALLY);
            
            return findList.Count == 0 ||
                   findList.Count(item => item.IsAlive()) == 0;
        }
    }
}