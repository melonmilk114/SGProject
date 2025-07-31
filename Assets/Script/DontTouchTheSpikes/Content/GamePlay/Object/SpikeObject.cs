using System.Collections;
using Melon;
using UnityEngine;

namespace DontTouchTheSpikes
{
    public class SpikeObject : GameElement
    {
        public Vector2 showPos = new Vector2();
        public Vector2 hidePos = new Vector2();
        
        private float duration = 0.2f;
        private Coroutine moveCoroutine = null;

        public void ShowSpike(bool isImmediately = false)
        {
            if(isImmediately)
            {
                transform.position = showPos;
            }
            else
            {
                if(moveCoroutine != null)
                    StopCoroutine(moveCoroutine);
                moveCoroutine = StartCoroutine(CoMoveSpike(showPos));
            }
        }
        
        public void HideSpike(bool isImmediately = false)
        {
            if(isImmediately)
            {
                transform.position = hidePos;
            }
            else
            {
                if(moveCoroutine != null)
                    StopCoroutine(moveCoroutine);
                moveCoroutine = StartCoroutine(CoMoveSpike(hidePos));
            }
        }

        public IEnumerator CoMoveSpike(Vector3 inEndPos)
        {
            //yield return new WaitForSeconds(0.1f);
            float timer = 0f;
            Vector3 currPos = transform.position;
            while (timer < duration)
            {
                timer += Time.deltaTime;
                transform.position = Vector3.Lerp(currPos, inEndPos, timer / duration);
                yield return null;
            }

            transform.position = inEndPos;
        }

        [ContextMenu("TempSetPos_LEFT")]
        public void TempSetPos_LEFT()
        {
            showPos = gameObject.transform.position;
            hidePos = showPos + new Vector2(-1, 0f);
        }
        
        [ContextMenu("TempSetPos_RIGHT")]
        public void TempSetPos_RIGHT()
        {
            showPos = gameObject.transform.position;
            hidePos = showPos + new Vector2(1, 0f);
        }
        
        [ContextMenu("TempSetPos_TOP")]
        public void TempSetPos_TOP()
        {
            showPos = gameObject.transform.position;
            hidePos = showPos + new Vector2(0f, 1f);
        }
        
        [ContextMenu("TempSetPos_BOTTOM")]
        public void TempSetPos_BOTTOM()
        {
            showPos = gameObject.transform.position;
            hidePos = showPos + new Vector2(0f, -1f);
        }
    }
}