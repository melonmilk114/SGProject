using System;
using System.Collections;
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
            gameManager?.SetRootObj(rootObj);
            gameManager?.InitManager();
            
            AddFrameworkModule(ObjectPoolManager.Instance);
            AddFrameworkModule(LanguageManager.Instance);
            AddFrameworkModule(ResourcesManager.Instance);

            InitFrameworkModule(new ActionResult()
            {
                onFail = (string inError) =>
                {
                    DebugLogHelper.LogError($"InitFrameworkModule Failed: {inError}");
                },
                onSuccess = () =>
                {
                    gameManager?.GameStart();
                }
            });
        }

        public void InitFrameworkModule(IActionResult inActionResult)
        {
            StartCoroutine(Co_InitFrameworkModule(inActionResult));
        }

        protected IEnumerator Co_InitFrameworkModule(IActionResult inActionResult)
        {
            int completedModules = 0;
            int totalModules = frameworkModules.Count;
    
            if (totalModules == 0)
            {
                inActionResult.OnSuccess();
                yield break;
            }

            for (int idx = 0; idx < totalModules; idx++)
            {
                var module = frameworkModules[idx];
                DebugLogHelper.Log($"InitModule Start: {module.GetType().Name} ({completedModules}/{totalModules})");
                yield return StartCoroutine(Co_InitSingleModule(module, (success, error) =>
                {
                    if (success)
                    {
                        completedModules++;
                        DebugLogHelper.Log($"InitModule Success: {module.GetType().Name} ({completedModules}/{totalModules})");
                
                        if (completedModules >= totalModules)
                        {
                            inActionResult.OnSuccess();
                        }
                    }
                    else
                    {
                        DebugLogHelper.LogError($"InitModule Failed: {module.GetType().Name} - {error}");
                        inActionResult.OnFail(error);
                        // 에러 발생 시 코루틴 중단
                        StopAllCoroutines();
                    }
                }));
            }
        }

        private IEnumerator Co_InitSingleModule(IFrameworkModule module, System.Action<bool, string> callback)
        {
            bool isCompleted = false;
            bool isSuccess = false;
            string errorMessage = string.Empty;
    
            module.InitModule(new ActionResult()
            {
                onSuccess = () =>
                {
                    isSuccess = true;
                    isCompleted = true;
                },
                onFail = (string error) =>
                {
                    isSuccess = false;
                    errorMessage = error;
                    isCompleted = true;
                }
            });
    
            // 모듈 초기화 완료까지 대기
            while (!isCompleted)
            {
                yield return null;
            }
    
            callback?.Invoke(isSuccess, errorMessage);
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