using UnityEngine;

namespace Melon
{
    public interface IActionResult
    {
        public void OnStart();
        public void OnSuccess();
        public void OnFail(string inError);
        public void OnEnd();
    }
}