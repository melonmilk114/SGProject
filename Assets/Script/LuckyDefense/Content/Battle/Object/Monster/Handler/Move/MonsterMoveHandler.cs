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
        
        public void Move(float inDeltaTime)
        {
            if (wayPoints.Count == 0) return;

            // 현재 목표 위치
            Vector3 target = wayPoints[currentWayPointIndex];

            // 목표 위치로 이동
            monster.transform.position = Vector3.MoveTowards(transform.position, target, tableData.speed * inDeltaTime);

            // 목표 위치에 도착하면 다음 위치로 변경
            if (Vector3.Distance(transform.position, target) < 0.1f)
            {
                currentWayPointIndex = (currentWayPointIndex + 1) % wayPoints.Count;
            }
        }
    }
}