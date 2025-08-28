

using UnityEngine;

namespace LuckyDefense
{
    public class MissileMoveHandler : MissileHandler
    {
        public void Move(float inDeltaTime)
        {
            Vector3 targetPos = missile.monsterTarget.transform.position;
            missile.transform.position = Vector3.MoveTowards(
                missile.transform.position,  // 현재 위치
                targetPos,                   // 목표 위치
                tableData.speed * inDeltaTime
            );
        }
    }
}