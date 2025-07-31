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
        
        public Action<long> onMissileLaunch = null;
        public float nextMissileLaunchTime = float.MaxValue;
        
        public void SetData(TowerTableDataItem inData)
        {
            tableData = inData;
            nextMissileLaunchTime = Time.time + tableData.missile_interval;
        }
        
        public void OnAttachTower(Action<long> inOnMissileLaunch)
        {
            onMissileLaunch = inOnMissileLaunch;
        }
        
        public void DoMissileLaunch(float inDeltaTime)
        {
            if (Time.time > nextMissileLaunchTime)
            {
                nextMissileLaunchTime = Time.time + tableData.missile_interval;
                onMissileLaunch?.Invoke(tableData.missile_sn);
            }
        }
    }
}