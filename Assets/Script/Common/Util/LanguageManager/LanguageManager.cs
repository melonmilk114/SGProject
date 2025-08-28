using UnityEngine;

namespace Melon
{
    public class LanguageManager : SingletonObject<LanguageManager>, IFrameworkModule
    {
        public string GetStr(int inKey, string inDefault)
        {
            return inDefault;
        }

        public void InitModule(IActionResult inActionResult)
        {
            inActionResult.OnSuccess();
        }
    }
}