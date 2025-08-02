using System.Linq;
using UnityEngine;

namespace GridHero.Battle
{
    [CreateAssetMenu(menuName = "Win Condition/All Enemy Die", fileName = "All Enemy Die", order = 0)]
    public class AllEnemyDie : BattleCheckCondition
    {
        public override bool CheckCondition()
        {
            if (battleContent == null)
            {
                DebugLogHelper.LogError("BattleContent is not set for AllEnemyDie condition.");
                return false;
            }
            
            var findList = battleContent.GetUnits(UNIT_FACTION.ENEMY);
            
            return findList.Count == 0 ||
                   findList.Count(item => item.IsAlive()) == 0;
        }
    }
}