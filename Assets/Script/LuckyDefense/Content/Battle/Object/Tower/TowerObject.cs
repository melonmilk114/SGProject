using System;
using Melon;
using UnityEngine;
using UnityEngine.Serialization;

namespace LuckyDefense
{
    public class TowerObject : GameElement
    {
        public TowerView view;
        public TowerTableDataItem tableData = null;
        
        public float nextMissileLaunchTime = float.MaxValue;
        
        public void SetData(TowerTableDataItem inData)
        {
            tableData = inData;
            nextMissileLaunchTime = Time.time + tableData.attack_speed;
            
            view.Init(tableData);
        }
        
        public bool IsMissileLaunchReady()
        {
            return Time.time > nextMissileLaunchTime;
        }

        public void DoMissileLaunch(MonsterObject inMonster)
        {
            nextMissileLaunchTime = Time.time + tableData.attack_speed;
            
            Vector2 dir = (inMonster.transform.position - transform.position).normalized;

            // 좌우/상하 중 큰 쪽으로 방향 고정
            if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
            {
                view.PlayAnimation(dir.x > 0 ? Vector2.right : Vector2.left);
            }
            else
            {
                view.PlayAnimation(dir.y > 0 ? Vector2.up : Vector2.down);
            }
        }
        
        public void DoTowerSpotMove(Vector3 inTowerSpotPos)
        {
            Vector2 dir = (inTowerSpotPos - transform.position).normalized;

            // 좌우/상하 중 큰 쪽으로 방향 고정
            if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
            {
                view.PlayAnimation(dir.x > 0 ? Vector2.right : Vector2.left);
            }
            else
            {
                view.PlayAnimation(dir.y > 0 ? Vector2.up : Vector2.down);
            }
        }
    }
}