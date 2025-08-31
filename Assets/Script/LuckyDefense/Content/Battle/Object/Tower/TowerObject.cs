using System;
using System.Collections.Generic;
using Melon;
using UnityEngine;
using UnityEngine.Serialization;

namespace LuckyDefense
{
    public class TowerObject : GameElement
    {
        public TowerView view;
        public StatObject stat;
        public TowerTableDataItem tableData = null;
        
        public long towerLevel = 1;
        public float nextMissileLaunchTime = float.MaxValue;
        
        public void SetData(TowerTableDataItem inData, long inTowerLevel, List<TowerLevelTableDataItem> inLevelDataList)
        {
            tableData = inData;
            towerLevel = inTowerLevel;
            nextMissileLaunchTime = Time.time + tableData.attack_speed;
            
            stat.InitStatObject(this, towerLevel, inData, inLevelDataList);
            
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
        
        [ContextMenu("Stat Calculate")]
        public void StatCalculate()
        {
            stat.CalculateStats(towerLevel);
            Debug.LogError($"[TowerObject] StatCalculate {tableData.sn} : " +
                           $"ATTACK={stat.GetStat(STAT_TYPE.ATTACK)}, " +
                           $"ATTACK_SPEED={stat.GetStat(STAT_TYPE.ATTACK_SPEED)}, " +
                           $"CRITICAL_RATE={stat.GetStat(STAT_TYPE.CRITICAL_RATE)}");
        }
    }
}