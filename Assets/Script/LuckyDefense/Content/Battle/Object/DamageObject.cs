using System;
using System.Collections;
using System.Collections.Generic;
using Melon;
using TMPro;
using UnityEngine;

namespace LuckyDefense
{
    public class DamageObject : GameElement, IObjectPoolUnit
    {
        public TextMeshPro label;
        
        public void SetDamage(long inDamage)
        {
            label.text = inDamage.ToString();
            StartCoroutine(FlyAndFade(3));
        }

        public IEnumerator FlyAndFade(float duration = 1.0f)
        {
            // 시작 위치와 목표 위치 (100,100 대각선)
            Vector3 startPos = transform.position;
            Vector3 endPos = startPos + new Vector3(1f, 1f, 0f);

            // 초기 색상 저장
            Color startColor = label.color;
            startColor.a = 1f;
            Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);

            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);

                // 위치 보간
                transform.position = Vector3.Lerp(startPos, endPos, t);

                // 색상 보간
                Color curColor = Color.Lerp(startColor, endColor, t);

                label.color = curColor;

                yield return null;
            }

            // 최종값 보장
            transform.position = endPos;
            label.color = endColor;
            
            yield return new WaitForSeconds(0.5f);
            
            ObjectPoolManager.Instance.EnqueuePool(this);
        }

        public void OnPoolDequeue()
        {
            
        }

        public void OnPoolEnqueue()
        {
            
        }
    }
}


