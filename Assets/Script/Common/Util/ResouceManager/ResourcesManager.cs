using Melon;
using UnityEngine;

namespace Melon
{
    public class ResourcesManager : MonoBehaviour, IFrameworkModule
    {
        #region Singleton
        private static ResourcesManager _instance = null;
        public static ResourcesManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<ResourcesManager>();
                }
                return _instance;
            }
        }
        #endregion
        
        public void InitModule()
        {
            
        }

        public Sprite GetSprite(string inPath)
        {
            Sprite sprite = Resources.Load<Sprite>(inPath);
            if (sprite == null)
            {
                Debug.LogError($"Load Sprite Failed: {inPath}");
            }
            return sprite;
        }
    }
}