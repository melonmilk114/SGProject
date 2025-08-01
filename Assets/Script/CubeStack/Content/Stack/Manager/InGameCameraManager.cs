using System.Collections;
using Melon;
using UnityEngine;

namespace CubeStack
{
    public class InGameCameraManager : GameElement
    {
        private Coroutine moveCoroutine;
        private float offestY = 0;
        
        public Camera inGameCamera;

        public void CameraPosReset()
        {
            float x = -1.5f;
            float y = 3f;
            float z = -1.5f;
            
            inGameCamera.transform.position = new Vector3(x, y, z);
        }
        
        public void GameReady()
        {
            CameraPosReset();
        }
        public void GameStart()
        {
            CameraPosReset();
        }

        public void MoveCamera(float inOffsetY)
        {
            // 현재 위치에서 이동 큐브의 y 크기인 0.1만큼 위로 이동
            offestY += inOffsetY;
            Vector3	start	= inGameCamera.transform.position;
            Vector3	end		= inGameCamera.transform.position + Vector3.up * offestY;

            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
                moveCoroutine = null;
            }

            moveCoroutine = StartCoroutine(CoMoveCamera(start, end, 0.25f));
        }
        
        private IEnumerator CoMoveCamera(Vector3 inStartPos, Vector3 inEndPos, float inMoveTime)
        {
            float current = 0;
            float percent = 0;

            while ( percent < 1 )
            {
                current += Time.deltaTime;
                percent = current / inMoveTime;
                
                var newPos = Vector3.Lerp(inStartPos, inEndPos, percent);
                offestY -= newPos.y - inGameCamera.transform.position.y;
                inGameCamera.transform.position = newPos;

                yield return null;
            }
        }
        
        // public void GameOverAnimation(float lastCubeY, UnityAction action=null)
        // {
        //     // 마지막에 배치한 lastCube의 y 위치가 limitMinY 보다 작으면 애니메이션 재생 X
        //     if ( limitMinY > lastCubeY )
        //     {
        //         if ( action != null ) action.Invoke();
        //
        //         return;
        //     }
        //
        //     // 카메라 y 위치 설정
        //     Vector3 startPosition	= transform.position;
        //     Vector3	endPosition		= new Vector3(transform.position.x, lastCubeY + 1, transform.position.z);
        //     // 카메라 이동 애니메이션
        //     StartCoroutine(OnMoveTo(startPosition, endPosition, gameOverAnimationTime));
        //
        //     // 카메라 View 크기 설정
        //     float startSize	= mainCamera.orthographicSize;
        //     float endSize	= lastCubeY - 1;
        //     // 카메라 View 크기 변경 애니메이션
        //     StartCoroutine(OnOrthographicSizeTo(startSize, endSize, gameOverAnimationTime, action));
        // }
    }
}