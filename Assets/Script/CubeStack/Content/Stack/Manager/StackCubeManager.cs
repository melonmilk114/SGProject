using System.Collections;
using System.Collections.Generic;
using Melon;
using Unity.VisualScripting;
using UnityEngine;

namespace CubeStack
{
    public class StackCubeManager : GameElement
    {
        public IStackContent stackContent;
        
        public Transform[] cubeSpawnPoints;
        public StackCube fallDropCubePrefab;
        public MoveStackCube moveStackCubePrefab;
        public List<StackCube> createCubeList = new List<StackCube>();

        public StackCube startTopCube;
        
        public Transform perfectStackEffectPrefab;
        public Transform perfectComboStackEffectPrefab;
        
        public Transform cubeScaleUpEffectPrefab;
        
        public int comboCount = 0;
        
        [HideInInspector]
        public StackCube lastCube = null;
        [HideInInspector]
        public MoveStackCube currentCube = null;
        
        private void OnDrawGizmos()
        {
            if (moveStackCubePrefab == null)
                return;
            
            for ( int i = 0; i < cubeSpawnPoints.Length; ++ i )
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireCube(cubeSpawnPoints[i].transform.position, moveStackCubePrefab.transform.localScale);
            }
        }

        public void GameStart()
        {
            lastCube = startTopCube;
            
            createCubeList.ForEach(inForItem => Destroy(inForItem.gameObject));
            createCubeList.Clear();
        }

        public StackCube SpawnCube(int inSpawnCount)
        {
            MoveStackCube spawnCube = Instantiate(moveStackCubePrefab);
            Transform spawnCubeTransform = spawnCube.transform;
            
            MOVE_DIR moveDir = inSpawnCount % cubeSpawnPoints.Length == 0 ? MOVE_DIR.DIR_X : MOVE_DIR.DIR_Z;
            
            float x = moveDir == MOVE_DIR.DIR_X ? cubeSpawnPoints[(int)moveDir].position.x : lastCube.transform.position.x;
            float y = lastCube.transform.position.y + (lastCube.transform.localScale.y / 2) + (spawnCube.transform.localScale.y / 2);
            float z = moveDir == MOVE_DIR.DIR_Z ? cubeSpawnPoints[(int)moveDir].position.z : lastCube.transform.position.z;
            
			
            spawnCubeTransform.position = new Vector3(x, y, z);
            spawnCubeTransform.localScale = new Vector3(lastCube.transform.localScale.x, moveStackCubePrefab.transform.localScale.y, lastCube.transform.localScale.z);
            
            spawnCube.SetData(moveDir);
            spawnCube.SetColor(GetRandomColor());
            spawnCube.MoveStart();
            
            createCubeList.Add(spawnCube);

            currentCube = spawnCube;

            return spawnCube;
        }
        
        // 큐브 쌓기
        public void StackCube()
        {
            if (currentCube == null)
                return;
            
            // 현재 큐브 이동 멈춤
            currentCube.MoveEnd();

            // MEMO : 잘 이해 안됨
            if ( IsGameOver(currentCube, lastCube) )
            {
                // 게임 오버
                stackContent.GameOver();
                return;
            }
		
            // 퍼펙트 여부 검사
            bool isPerfect = IsPerfectStack(currentCube, lastCube);

            if (isPerfect)
            {
                // 퍼펙트 스택
                float x = lastCube.transform.position.x;
                float y = lastCube.transform.position.y + currentCube.transform.localScale.y;
                float z = lastCube.transform.position.z;
                
                currentCube.transform.position = new Vector3(x, y, z);

                if(comboCount < 2)
                {
                    // 일반 이펙트
                    PerfectStackEffect(currentCube.transform);
                }
                else if(comboCount < 5)
                {
                    // 콤보 이펙트
                    PerfectComboStackEffect(currentCube.transform);
                }
                else
                {
                    // 큐브 증가 이펙트 및 큐브 크기 증가
                    PerfectComboStackEffect(currentCube.transform);
                    CubeScaleUpEffect(currentCube.transform);
                    CubeScaleUp(currentCube);
                }

                comboCount++;
            }
            else
            {
                comboCount = 0;
                float hangOver = GetCubeHangOver(currentCube, lastCube);

                if (currentCube.moveDir == MOVE_DIR.DIR_X)
                {
                    SplitCubeOnX(currentCube, hangOver);
                }
                else
                {
                    SplitCubeOnZ(currentCube, hangOver);
                }
            }

            stackContent.AddScore(10);
            
            lastCube = currentCube;
        }

        private Color GetRandomColor()
        {
            // MEMO : 고민좀 해보자
            float colorAmount = (1.0f/255.0f) * 15.0f;
            var color = lastCube.GetColor();
            color = new Color(color.r - colorAmount, color.g - colorAmount, color.b - colorAmount);
            
            
            Color.RGBToHSV(color, out float h, out _, out _);

            // 비슷한 색조 유지 + 랜덤 파스텔톤
            h += Random.Range(-0.05f, 0.05f);
            h = Mathf.Repeat(h, 1f); // Hue를 0~1 범위로 유지

            float s = Random.Range(0.3f, 0.5f); // 낮은 채도
            float v = Random.Range(0.85f, 1.0f); // 밝기 유지

            return Color.HSVToRGB(h, s, v);

            //return color;
        }

        private float GetCubeHangOver(MoveStackCube inTargetCube, StackCube inReferenceCube)
        {
            float amount = 0;
            
            if ( inTargetCube.moveDir == MOVE_DIR.DIR_X)
            {
                amount = inTargetCube.transform.position.x - inReferenceCube.transform.position.x;
            }
            else if ( inTargetCube.moveDir == MOVE_DIR.DIR_Z)
            {
                amount = inTargetCube.transform.position.z - inReferenceCube.transform.position.z;
            }

            return amount;
        }
        
        private bool IsGameOver(MoveStackCube inTargetCube, StackCube inReferenceCube)
        {
            float hangOver = GetCubeHangOver(inTargetCube, inReferenceCube);
            float max = inTargetCube.moveDir == MOVE_DIR.DIR_X? inReferenceCube.transform.localScale.x :
                inReferenceCube.transform.localScale.z;

            if ( Mathf.Abs(hangOver) > max )
            {
                // 게임 오버
                return true;
            }

            return false;
        }
        
        private bool IsPerfectStack(MoveStackCube inTargetCube, StackCube inReferenceCube)
        {
            float hangOver = GetCubeHangOver(inTargetCube, inReferenceCube);
            
            if ( Mathf.Abs(hangOver) < 0.01f )
            {
                // 퍼펙스 스택
                return true;
            }

            return false;
        }
        
        private void CubeScaleUp(MoveStackCube inTargetCube)
        {
            // 이동 방향으로 커지게함
            float recoverySize = 0.1f;

            if ( inTargetCube.moveDir == MOVE_DIR.DIR_X)
            {
                float newXSize			= inTargetCube.transform.localScale.x + recoverySize;
                float newXPosition		= inTargetCube.transform.position.x + recoverySize * 0.5f;

                inTargetCube.transform.position		= new Vector3(newXPosition, inTargetCube.transform.position.y, inTargetCube.transform.position.z);
                inTargetCube.transform.localScale	= new Vector3(newXSize, inTargetCube.transform.localScale.y, inTargetCube.transform.localScale.z);
            }
            else
            {
                float newZSize			= inTargetCube.transform.localScale.z + recoverySize;
                float newZPosition		= inTargetCube.transform.position.z + recoverySize * 0.5f;

                inTargetCube.transform.position		= new Vector3(inTargetCube.transform.position.x, inTargetCube.transform.position.y, newZPosition);
                inTargetCube.transform.localScale	= new Vector3(inTargetCube.transform.localScale.x, inTargetCube.transform.localScale.y, newZSize);
            }
        }
        
        private void SplitCubeOnX(MoveStackCube inTargetCube, float inHangOver)
        {
            var fallDirection = inHangOver >= 0 ? 1 : -1;
            var targetTransform = inTargetCube.transform;
            // 이동 큐브의 새로운 위치, 크기 연산
            float newXPosition = targetTransform.position.x - (inHangOver / 2);
            float newXSize = targetTransform.localScale.x - Mathf.Abs(inHangOver);
            // 이동 큐브의 위치, 크기 설정
            targetTransform.position = new Vector3(newXPosition, targetTransform.position.y, targetTransform.position.z);
            targetTransform.localScale = new Vector3(newXSize, targetTransform.localScale.y, targetTransform.localScale.z);

            // 조각 큐브의 위치, 크기 연산
            float cubeEdge = targetTransform.position.x + (targetTransform.localScale.x / 2 * fallDirection);
            float fallingBlockSize = Mathf.Abs(inHangOver);
            float fallingBlockPosition = cubeEdge + fallingBlockSize / 2 * fallDirection;
            // 조각 큐브 생성
            SpawnDropCube(new Vector3(fallingBlockPosition, targetTransform.position.y, targetTransform.position.z), 
                new Vector3(fallingBlockSize, targetTransform.localScale.y, targetTransform.localScale.z),
                inTargetCube.GetColor());
        }
        
        private void SplitCubeOnZ(MoveStackCube inTargetCube, float inHangOver)
        {
            var fallDirection = inHangOver >= 0 ? 1 : -1;
            var targetTransform = inTargetCube.transform;
            // 이동 큐브의 새로운 위치, 크기 연산
            float newZPosition			= targetTransform.position.z - (inHangOver / 2);
            float newZSize				= targetTransform.localScale.z - Mathf.Abs(inHangOver);
            // 이동 큐브의 위치, 크기 설정
            targetTransform.position			= new Vector3(targetTransform.position.x, targetTransform.position.y, newZPosition);
            targetTransform.localScale		= new Vector3(targetTransform.localScale.x, targetTransform.localScale.y, newZSize);

            // 조각 큐브의 위치, 크기 연산
            float cubeEdge				= targetTransform.position.z + (targetTransform.localScale.z / 2 * fallDirection);
            float fallingBlockSize		= Mathf.Abs(inHangOver);
            float fallingBlockPosition	= cubeEdge + fallingBlockSize / 2 * fallDirection;
            // 조각 큐브 생성
            SpawnDropCube(new Vector3(targetTransform.position.x, targetTransform.position.y, fallingBlockPosition), 
                new Vector3(targetTransform.localScale.x, targetTransform.localScale.y, fallingBlockSize),
                inTargetCube.GetColor());
        }
        
        private void SpawnDropCube(Vector3 inPos, Vector3 inSize, Color inColor)
        {
            StackCube fallDropCube = Instantiate(fallDropCubePrefab);
            Transform fallDropCubeTransform = fallDropCube.transform;

            // 방금 생성한 조각 큐브의 위치, 크기 설정
            fallDropCubeTransform.position = inPos;
            fallDropCubeTransform.localScale = inSize;
            fallDropCube.SetColor(inColor);
            fallDropCube.gameObject.AddComponent<Rigidbody>();

            // 2초 뒤에 삭제
            Destroy(fallDropCube.gameObject, 2);
        }
        
        public void PerfectStackEffect(Transform inTargetCube)
        {
            if (inTargetCube == null)
                return;

            // 퍼펙트 스택 이펙트 생성
            Transform effect = Instantiate(perfectStackEffectPrefab);
            
            float x = inTargetCube.position.x;
            float y = inTargetCube.position.y - (inTargetCube.localScale.y / 2);
            float z = inTargetCube.position.z;
            effect.position = new Vector3(x, y, z);
            
            x = inTargetCube.localScale.x + 0.1f;
            y = inTargetCube.localScale.y;
            z = inTargetCube.localScale.z + 0.1f;
            effect.localScale = new Vector3(x, y, z);

            var fadeEffect = effect.AddComponent<FadeEffect>();
            var effectMono = effect.GetComponent<MonoBehaviour>();
            effectMono?.StartCoroutine(effectMono.WhenAll(() =>
            {
                Destroy(effect.gameObject, 1.0f);
            },fadeEffect.CoStartEffect()));
        }
        
        public void PerfectComboStackEffect(Transform inTargetCube)
        {
            if (inTargetCube == null)
                return;

            var effectCount = comboCount < 5 ? comboCount : 5;
            for (int idx = 0; idx < effectCount; idx++)
            {
                // 퍼펙트 스택 이펙트 생성
                Transform effect = Instantiate(perfectComboStackEffectPrefab);
            
                float x = inTargetCube.position.x;
                float y = inTargetCube.position.y - (inTargetCube.localScale.y / 2);
                float z = inTargetCube.position.z;
                effect.position = new Vector3(x, y, z);
            
                x = inTargetCube.localScale.x + 0.1f;
                y = inTargetCube.localScale.y;
                z = inTargetCube.localScale.z + 0.1f;
                effect.localScale = new Vector3(x, y, z);
                
                var fadeEffect = effect.AddComponent<FadeEffect>();
                var scaleEffect = effect.AddComponent<ScaleEffect>();
                scaleEffect.SetDelay(0.15f * idx);
                var effectMono = effect.GetComponent<MonoBehaviour>();
                effectMono?.StartCoroutine(effectMono.WhenAll(() =>
                {
                    Destroy(effect.gameObject, 1.0f);
                },fadeEffect.CoStartEffect(), scaleEffect.CoStartEffect()));
            }
        }
        
        public void CubeScaleUpEffect(Transform inTargetCube)
        {
            // 이펙트 생성
            Transform effect = Instantiate(cubeScaleUpEffectPrefab);
            // 이펙트 생성 위치
            effect.position = inTargetCube.position;

            // 이펙트의 생성 반경 설정 (반지름과 두께)
            var   shape  = effect.GetComponent<ParticleSystem>().shape;
            float radius = inTargetCube.localScale.x > inTargetCube.localScale.z ?
                inTargetCube.localScale.x : inTargetCube.localScale.z;
            shape.radius = radius;
            shape.radiusThickness = radius * 0.5f;
            
            Destroy(effect.gameObject, 2.0f);
        }
        
        #region TestFunc

        public void PerfectStackTest()
        {
            if (currentCube == null)
                return;

            // 퍼펙트 스택
            currentCube.transform.position = lastCube.transform.position;
            
            StackCube();
        }
        

        #endregion

    }
}