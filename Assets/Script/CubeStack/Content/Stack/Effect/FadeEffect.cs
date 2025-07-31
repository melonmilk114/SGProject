using System.Collections;
using Melon;
using UnityEngine;

namespace CubeStack
{
    public class FadeEffect : GameElement
    {
        [SerializeField]
        private	float duration = 0.8f;
        private	MeshRenderer meshRenderer;

        public override void OnAwakeFunc()
        {
            base.OnAwakeFunc();
            meshRenderer = GetComponent<MeshRenderer>();
        }

        public IEnumerator CoStartEffect()
        {
            float current = 0;
            float percent = 0;

            float start = meshRenderer.material.color.a;
            float end	= 0;

            while ( percent < 1 )
            {
                current += Time.deltaTime;
                percent = current / duration;

                Color color				= meshRenderer.material.color;
                color.a					= Mathf.Lerp(start, end, percent);
                meshRenderer.material.color	= color;

                yield return null;
            }
        }
    }
}