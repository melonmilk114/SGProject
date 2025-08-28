using System.Collections.Generic;
using UnityEngine;

namespace Melon
{
    public static class TargetObjectReceiver
    {
        public static void DoTargetObjectInject<T>(T inTarget, GameObject inParent)
        {
            var interfaceList = new List<ITargetObjectReceiver<T>>();
            
            if (inParent != null)
            {   
                interfaceList.AddRange(inParent.GetComponentsInChildren<ITargetObjectReceiver<T>>(true));
            }
            else
            {  
                var rootGameObjects = new List<GameObject>(UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects());
                rootGameObjects.ForEach(rootGameObject =>
                {
                    interfaceList.AddRange(rootGameObject.GetComponentsInChildren<ITargetObjectReceiver<T>>(true));
                });
            }
                
            // DontDestroyOnLoad 탐색
            var allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
            foreach (var obj in allObjects)
            {
                // DontDestroyOnLoad에 있는 오브젝트인지 확인
                if (obj.scene.name == "DontDestroyOnLoad")
                {
                    interfaceList.AddRange(obj.GetComponentsInChildren<ITargetObjectReceiver<T>>(true));
                }
            }
            
            interfaceList.ForEach(child =>
            {
                child.GetTargetObject = inTarget;
                child.SetTargetObject(inTarget);
            });
        }
    }
}