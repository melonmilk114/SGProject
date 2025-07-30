using System.Collections.Generic;
using UnityEngine;

namespace Melon
{
    public static class TargetObjectReceiver
    {
        public static void DoTargetObjectInject<T>(T inTarget, GameObject inParent)
        {
            var interfaceList = new List<ITargetObjectReceiver<T>>();
            {
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
            }
            interfaceList.ForEach(child =>
            {
                child.GetTargetObject = inTarget;
                child.SetTargetObject(inTarget);
            });
        }
    }
}