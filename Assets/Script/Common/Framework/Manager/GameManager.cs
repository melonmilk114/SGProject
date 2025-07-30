using UnityEngine;
using UnityEngine.Serialization;

namespace Melon
{
    public class GameManager : Manager
    {
        public TableDataManager tableDataManager = null;
        public DataManager dataManager = null;
        public ContentManager contentManager = null;
        
        public override void InitManager()
        {
            tableDataManager.SetRootObj(rootObj);
            tableDataManager.InitManager();
            
            dataManager.SetRootObj(rootObj);
            dataManager.InitManager();
            
            contentManager.SetRootObj(rootObj);
            contentManager.InitManager();
        }

        public virtual void GameStart(object inData = null)
        {
            
        }

        public virtual void GameEnd(object inData = null)
        {
            
        }
    }
}