using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace LuckyDefense
{
    public class MonsterHandler : MonoBehaviour, IMonsterHandler
    {
        public MonsterObject monster;
        public MonsterTableDataItem tableData;
        public void Setup(MonsterObject inMonster)
        {
            monster = inMonster;
            tableData = monster.tableData;
        }

        public virtual void StartHandler()
        {
            
        }
        
        
    }
}