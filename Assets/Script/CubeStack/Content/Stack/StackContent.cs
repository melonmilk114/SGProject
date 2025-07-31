using System;
using System.Collections;
using CubeStack.UI;
using UnityEngine;

namespace CubeStack
{
    public class StackContent : Content
    , IStackContent
    {
        public StackCubeManager stackCubeManager;
        public InGameCameraManager inGameCameraManager;
        
        public StackContentOutGameCanvas stackContentOutGameCanvas;
        
        private int spawnCubeCount = 0;
        private GAME_STATE gameState = GAME_STATE.NONE;
        private Coroutine gameStartCoroutine;
        
        private int score = 0;
        
        public override Enum GetContentType()
        {
            return ContentManager.ContentType.STACK;
        }
        
        public override void InitContent()
        {
            base.InitContent();

            stackCubeManager.stackContent = this;
            stackContentOutGameCanvas.stackContent = this;
        }
        
        public override void DoContentStart(object inData)
        {
            GameReady();
        }

        public void AddScore(int inScore)
        {
            score += inScore;
            
            stackContentOutGameCanvas.UpdateScore(score);
        }

        public void GameReady()
        {
            gameState = GAME_STATE.READY;
            
            stackContentOutGameCanvas?.UpdateGameState();
        }

        public void GameStart()
        {
            if (gameStartCoroutine != null)
            {
                StopCoroutine(gameStartCoroutine);
            }

            gameState = GAME_STATE.PLAYING;
            
            stackContentOutGameCanvas?.UpdateGameState();

            score = 0;
            spawnCubeCount = 0;;
            inGameCameraManager.GameStart();
            stackCubeManager.GameStart();
            gameStartCoroutine = StartCoroutine(CoGameStart());
            
            var spawnCube = stackCubeManager.SpawnCube(spawnCubeCount);
            spawnCubeCount++;
        }

        public void GameOver()
        {
            gameState = GAME_STATE.GAMEOVER;
            
            stackContentOutGameCanvas?.UpdateGameState();
            
            if (gameStartCoroutine != null)
            {
                StopCoroutine(gameStartCoroutine);
                gameStartCoroutine = null;
            }
        }

        public GAME_STATE GetGameState()
        {
            return gameState;
        }

        private IEnumerator CoGameStart()
        {
            while (true)
            {
                // Perfect 테스트용
                if ( Input.GetMouseButtonDown(1) )
                {
                    stackCubeManager.PerfectStackTest();
                    
                    var spawnCube = stackCubeManager.SpawnCube(spawnCubeCount);
                    spawnCubeCount++;
                    
                    // 카메라 이동
                    inGameCameraManager?.MoveCamera(spawnCube.transform.localScale.y);
                }

                if (Input.GetMouseButtonDown(0))
                {
                    if (gameState != GAME_STATE.PLAYING)
                        yield return null;
                    
                    stackCubeManager.StackCube();

                    if (gameState == GAME_STATE.GAMEOVER)
                        yield break;

                    var spawnCube = stackCubeManager.SpawnCube(spawnCubeCount);
                    spawnCubeCount++;
                    
                    // 카메라 이동
                    inGameCameraManager?.MoveCamera(spawnCube.transform.localScale.y);
                }

                yield return null;
            }
        }
    }
}