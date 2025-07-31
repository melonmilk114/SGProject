using UnityEngine;

namespace LuckyDefense
{
    public class MonsterObjectState : MonoBehaviour
    {
        public MonsterTableDataItem tableData = null;
        
        public bool isPreDead = false;
        public bool isDead = false;
        public long preHp = 0;
        public long hp = 0;

        public float hpRatio
        {
            get
            {
                if (tableData == null)
                    return 0;
                
                return (float)hp / (float)tableData.hp;
            }
        }
        
        public void Init(MonsterTableDataItem inData)
        {
            tableData = inData;
            
            preHp = tableData.hp;
            hp = tableData.hp;
            isPreDead = false;
            isDead = false;
        }
        
        public void Reset()
        {
            isPreDead = true;
            isDead = true;
            preHp = 0;
            hp = 0;
        }
        // 미사일 선판정
        public void PreDamage(long inDamage)
        {
            if (isPreDead)
                return;
            
            preHp -= inDamage;
            if (preHp <= 0)
            {
                isPreDead = true;
            }
        }
        public void MissileHit(long inDamage)
        {
            if (isDead)
                return;
            
            hp -= inDamage;
            if (hp <= 0)
            {
                isDead = true;
            }
        }
    }
}