using System;
using System.Collections;
using UnityEngine;

namespace Melon
{
    public static class CoroutineExtension
    {
        // 코루틴이 끝나면 완료 콜백 호출
        public static IEnumerator TrackCoroutine(IEnumerator coroutine, System.Action onComplete)
        {
            yield return coroutine;
            onComplete?.Invoke();
        }
        
        // 모든 코루틴이 끝날때 까지 대기하는 코루틴
        public static IEnumerator WhenAll(this MonoBehaviour mono, params IEnumerator[] coroutines)
        {
            int coroutineCount = coroutines.Length;
            int completedCount = 0;

            foreach (var coroutine in coroutines)
            {
                mono.StartCoroutine(TrackCoroutine(coroutine, () => completedCount++));
            }

            while (completedCount < coroutineCount)
            {
                yield return null;
            }
        }
        // 모든 코루틴이 끝날때 까지 대기하고 모두 끝나면 콜백 함수 호출
        public static IEnumerator WhenAll(this MonoBehaviour mono, Action finishAction, params IEnumerator[] coroutines)
        {
            int coroutineCount = coroutines.Length;
            int completedCount = 0;

            foreach (var coroutine in coroutines)
            {
                mono.StartCoroutine(TrackCoroutine(coroutine, () => completedCount++));
            }

            while (completedCount < coroutineCount)
            {
                yield return null;
            }
            
            finishAction?.Invoke();
        }
        
        // 모든 코루틴이 순차적으로 실행함
        public static IEnumerator ConsecutiveAll(this MonoBehaviour mono, params IEnumerator[] coroutines)
        {
            foreach (var coroutine in coroutines)
            {
                yield return coroutine;
            }
        }
    }
}