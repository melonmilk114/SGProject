using System;
using UnityEngine;

namespace Melon
{
    public class ActionResult : IActionResult
    {
        public Action onStart = null;
        public Action onSuccess = null;
        public Action<string> onFail = null;
        public Action onEnd = null;
        public Action<object> onReturnObject = null;

        public ActionResult(Action inOnStart = null,
            Action inOnSuccess = null,
            Action<string> inOnFail = null,
            Action inOnEnd = null,
            Action<object> inOnReturnObject = null)
        {
            onStart = inOnStart;
            onSuccess = inOnSuccess;
            onFail = inOnFail;
            onEnd = inOnEnd;
            onReturnObject = inOnReturnObject;
        }
        
        public void OnStart()
        {
            onStart?.Invoke();
        }

        public void OnSuccess()
        {
            onSuccess?.Invoke();
        }

        public void OnFail(string inError)
        {
            onFail?.Invoke(inError);
        }

        public void OnEnd()
        {
            onEnd?.Invoke();
        }
        
        public void OnReturnObject(object inResult)
        {
            onReturnObject.Invoke(inResult);
        }
    }
}