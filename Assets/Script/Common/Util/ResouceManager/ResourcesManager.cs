using Melon;
using UnityEngine;

namespace Melon
{
    public class ResourcesManager : SingletonObject<ResourcesManager>, IFrameworkModule
    {
        public void InitModule(IActionResult inActionResult)
        {
            inActionResult.OnSuccess();
        }

        public GameElement LoadGameElement(string inPath)
        {
            GameElement gameElement = Resources.Load<GameElement>(inPath);
            if (gameElement == null)
            {
                DebugLogHelper.LogError($"Load GameElement Failed: {inPath}");
            }
            return gameElement;
        }

        public Sprite GetSprite(string inPath)
        {
            Sprite sprite = Resources.Load<Sprite>(inPath);
            if (sprite == null)
            {
                DebugLogHelper.LogError($"Load Sprite Failed: {inPath}");
            }
            return sprite;
        }
        
        public RuntimeAnimatorController GetAnimatorController(string inPath)
        {
            RuntimeAnimatorController controller = Resources.Load<RuntimeAnimatorController>(inPath);
            if (controller == null)
            {
                DebugLogHelper.LogError($"Load AnimatorController Failed: {inPath}");
            }
            return controller;
        }
    }
}