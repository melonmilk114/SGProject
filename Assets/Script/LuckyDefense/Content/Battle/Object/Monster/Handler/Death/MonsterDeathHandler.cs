using LuckyDefense;
using Melon;
using UnityEngine;

namespace LuckyDefense
{
    public class MonsterDeathHandler : MonsterHandler
    {
        public void Death(ActionResult inActionResult)
        {
            inActionResult.OnSuccess();
        }
    }
}