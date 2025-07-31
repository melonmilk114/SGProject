using Melon;
using UnityEngine;

namespace CubeStack
{
    public class TableDataManager : Melon.TableDataManager
    {
        public override void InitManager()
        {
            base.InitManager();

            TargetObjectReceiver.DoTargetObjectInject(this, rootObj);
        }
        
        public override void InitData()
        {
            base.InitData();
          
        }

        public override void ResetData()
        {
            base.ResetData();
            
        }
    }
}