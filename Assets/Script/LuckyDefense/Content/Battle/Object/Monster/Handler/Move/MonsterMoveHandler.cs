using System.Collections.Generic;
using UnityEngine;

namespace LuckyDefense
{
    public class MonsterMoveHandler : MonsterHandler
    {
        protected List<Vector3> wayPoints = new List<Vector3>();
        protected int currentWayPointIndex = 0;
        
        public void SetWayPoint(List<Vector3> inPoints)
        {
            wayPoints = inPoints;
        }
        
        public virtual void StartHandler()
        {
            currentWayPointIndex = 0;
        }
        
        public Vector2 Move(float inDeltaTime)
        {
            if (wayPoints.Count == 0) return Vector2.zero;

            Vector3 target = wayPoints[currentWayPointIndex];

            // 방향 벡터 (목표 - 현재)
            Vector3 dir = (target - transform.position).normalized;

            // 목표 위치로 이동
            monster.transform.position = Vector3.MoveTowards(
                transform.position,
                target,
                tableData.speed * inDeltaTime
            );

            // 목표 지점 도착 시 다음 웨이포인트로
            if (Vector3.Distance(transform.position, target) < 0.1f)
            {
                currentWayPointIndex = (currentWayPointIndex + 1) % wayPoints.Count;
            }
            
            // 4방향 판정
            if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
            {
                return dir.x > 0 ? Vector2.right : Vector2.left;
            }
            else
            {
                return dir.y > 0 ? Vector2.up : Vector2.down;
            }
        }
    }
}