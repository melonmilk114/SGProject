using UnityEngine;

namespace Melon
{
    public class LanguageManager : MonoBehaviour, IFrameworkModule
    {
        #region Singleton
        private static LanguageManager _instance = null;
        public static LanguageManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<LanguageManager>();
                }
                return _instance;
            }
        }
        #endregion

        public string GetStr(int inKey, string inDefault)
        {
            return inDefault;
        }

        public void InitModule()
        {
            
        }
    }
}