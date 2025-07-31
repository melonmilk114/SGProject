

using UnityEngine;

namespace LuckyDefense
{
    public class MissileMoveHandler : MissileHandler
    {
        public void Move(float inDeltaTime)
        {
            Vector3 targetPos = missile.monsterTarget.transform.position;
            Vector3 moveDir = targetPos - transform.position;
            moveDir.Normalize();
            missile.transform.Translate(moveDir * tableData.speed * inDeltaTime);
        }
    }
}