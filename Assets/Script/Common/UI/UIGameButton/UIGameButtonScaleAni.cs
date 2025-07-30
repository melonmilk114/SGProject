using System;
using System.Collections;
using UnityEngine;

namespace Melon
{
    // TODO : DoTween을 사용 하도록 수정 필요 우선 코루틴으로 제작
    public class UIGameButtonScaleAni : MonoBehaviour
    {
        [SerializeField] private Vector3 pressedScale = new Vector3(0.9f, 0.9f, 0.9f); // 눌렀을 때 스케일
        [SerializeField] private float duration = 0.1f; // 스케일 변경 애니메이션 시간
        
        private RectTransform rect;
        private Vector3 defaultScale;
        private Coroutine aniCoroutine = null;
        
        private void Awake()
        {
            rect = GetComponent<RectTransform>();
            defaultScale = rect.localScale;
        }
        
        void OnDisable()
        {
            SetLocalScale(defaultScale);
        }

        private void SetLocalScale(Vector3 inScale)
        {
            rect.localScale = inScale;
        }
        
        public void OnPointerDown()
        {
            if (aniCoroutine != null)
            {
                StopCoroutine(aniCoroutine);
                SetLocalScale(defaultScale);
            }
            aniCoroutine = StartCoroutine(CoPointDown());
        }

        public void OnPointerUp()
        {
            if (aniCoroutine != null)
            {
                StopCoroutine(aniCoroutine);
                SetLocalScale(pressedScale);
            }
            aniCoroutine = StartCoroutine(CoPointUp());
        }
        
        private IEnumerator CoPointDown()
        {
            float timer = 0f;
            Vector3 startScale = rect.localScale;
            while (timer < duration)
            {
                timer += Time.deltaTime;
                rect.localScale = Vector3.Lerp(startScale, pressedScale, timer / duration);
                yield return null;
            }
            rect.localScale = pressedScale;
        }

        private IEnumerator CoPointUp()
        {
            float timer = 0f;
            Vector3 startScale = rect.localScale;
            while (timer < duration)
            {
                timer += Time.deltaTime;
                rect.localScale = Vector3.Lerp(startScale, defaultScale, timer / duration);
                yield return null;
            }
            rect.localScale = defaultScale;
        }
    }
}