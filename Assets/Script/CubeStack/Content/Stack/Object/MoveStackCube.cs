using System.Collections;
using UnityEngine;

namespace CubeStack
{
    public class MoveStackCube : StackCube
    {
        public float moveSpeed = 1.0f;
        public MOVE_DIR moveDir = MOVE_DIR.DIR_X;
        public Vector3 moveDirVec;
        
        private Coroutine moveCoroutine = null;
        
        public void SetData(MOVE_DIR inDir)
        {
            moveDir = inDir;
        }
        
        public void MoveStart()
        {
            MoveEnd();
            moveCoroutine = StartCoroutine(Co_Move());
        }

        private IEnumerator Co_Move()
        {
            while (true)
            {
                transform.position += moveDirVec * moveSpeed * Time.deltaTime;

                if ( moveDir == MOVE_DIR.DIR_X )
                {
                    if ( transform.position.x <= -1.5f )		moveDirVec = Vector3.right;
                    else if ( transform.position.x >= 1.5f )	moveDirVec = Vector3.left;
                }
                else if ( moveDir == MOVE_DIR.DIR_Z )
                {
                    if ( transform.position.z <= -1.5f )		moveDirVec = Vector3.forward;
                    else if ( transform.position.z >= 1.5f )	moveDirVec = Vector3.back;
                }
                
                yield return null;
            }
        }

        public void MoveEnd()
        {
            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
                moveCoroutine = null;
            }
        }
    }
}