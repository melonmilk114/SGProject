using System;
using System.Collections.Generic;
using UnityEngine;

namespace Melon
{
    public class GameFramework : GameElement
    {
        public GameObject rootObj;
        public GameManager gameManager = null;
        
        public List<IFrameworkModule> frameworkModules = new List<IFrameworkModule>();

        public virtual void InitFramework()
        {
            ObjectPoolManager.Instance.InitModule();
            LanguageManager.Instance.InitModule();
            ResourcesManager.Instance.InitModule();
            
            gameManager?.SetRootObj(rootObj);
            gameManager?.InitManager();
            
            frameworkModules?.ForEach(item => item.InitModule());
        }

        public void AddFrameworkModule(IFrameworkModule inModule)
        {
            frameworkModules.Add(inModule);
        }
        
        public void GameStart(object inData = null)
        {
            gameManager?.GameStart(inData);
        }

        public void GameEnd(object inData = null)
        {
            gameManager?.GameEnd(inData);
        }
    }
}