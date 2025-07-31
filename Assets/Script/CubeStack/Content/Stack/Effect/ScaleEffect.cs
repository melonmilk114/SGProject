using System.Collections;
using UnityEngine;

namespace CubeStack
{
    public class ScaleEffect : MonoBehaviour
    {
        public float delay = 0f;
        [SerializeField] 
        private float duration = 0.4f;

        public void SetDelay(float inDelay)
        {
            delay = inDelay;
        }
        
        public IEnumerator CoStartEffect()
        {
            yield return new WaitForSeconds(delay);
            
            float current = 0;
            float percent = 0;

            Vector3 start = transform.localScale;
            Vector3 end = new Vector3(transform.localScale.x * 1.5f,
                transform.localScale.y,
                transform.localScale.z * 1.5f);

            while (percent < 1)
            {
                current += Time.deltaTime;
                percent = current / duration;

                transform.localScale = Vector3.Lerp(start, end, percent);

                yield return null;
            }
        }
    }
}