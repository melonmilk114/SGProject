using UnityEngine;

namespace Melon
{
    public class Manager : MonoBehaviour
    {
        [HideInInspector] public GameObject rootObj;

        public void SetRootObj(GameObject inObj)
        {
            rootObj = inObj;
        }
        
        public virtual void InitManager()
        {
            
        }
    }
}