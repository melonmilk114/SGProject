using UnityEngine;

namespace LuckyDefense
{
    public class MissileHandler : MonoBehaviour, IMissileHandler
    {
        public MissileObject missile;
        public MissileTableDataItem tableData;
        
        public void Setup(MissileObject inMissile)
        {
            missile = inMissile;
            tableData = missile.tableData;
        }

        public virtual void StartHandler()
        {
            
        }
    }
}