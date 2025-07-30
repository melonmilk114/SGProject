using UnityEngine;

namespace Melon
{
    public class DataManager : Manager
    {
        public override void InitManager()
        {
            base.InitManager();
            InitData();
        }
        public virtual void InitData()
        {
            
        }

        public virtual void ResetData()
        {
            
        }
    }
}