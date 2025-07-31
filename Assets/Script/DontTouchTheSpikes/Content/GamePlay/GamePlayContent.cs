using System;
using System.Collections;
using System.Collections.Generic;
using DontTouchTheSpikes;
using DontTouchTheSpikes.UI;
using UnityEngine;

namespace DontTouchTheSpikes
{
    public class GamePlayContent : Content
    , IGamePlayContent
    {
        public GamePlaySpikeManager spikeManager;
        public PlayerObject player;

        public int score = 0;
        public GAME_STATE gameState = GAME_STATE.NONE;
        
        public GamePlayOutGameCanvas gamePlayOutGameCanvas;
        public GamePlayInGameCanvas gamePlayInGameCanvas;

        private Coroutine gamePlayCoroutine = null;

        public override void OnAwakeFunc()
        {
            base.OnAwakeFunc();
            
            gamePlayOutGameCanvas.gamePlayContent = this;
            player.onPlayerTriggerEnter = OnPlayerTriggerEnter;
        }

        public override Enum GetContentType()
        {
            return ContentManager.ContentType.GAMEPLAY;
        }
        
        public GAME_STATE GetGameState()
        {
            return gameState;
        }

        public override void DoPostShow(object inData = null)
        {
            GameReady();
        }
        
        public void GameReady()
        {
            DebugLogHelper.Log("GameReady");
            StopGamePlayCoroutine();
            gameState = GAME_STATE.READY;
            score = 0;
            player.GameReady();
            
            gamePlayOutGameCanvas.UpdateGameState();
            gamePlayInGameCanvas.UpdateScore(score);
        }
        public void GameStart()
        {
            DebugLogHelper.Log("GameStart");
            StopGamePlayCoroutine();
            gameState = GAME_STATE.PLAYING;
            score = 0;
            gamePlayCoroutine = StartCoroutine(CoGamePlaying());
            player.GameStart();

            spikeManager.AllShowSpike(true);
            var showWallDir = CommonFunc.ConvertToPlayerMoveDirToWallDir(player.GetHorizontalDirection());
            spikeManager.AllHideSpike(showWallDir, true);
            
            gamePlayOutGameCanvas.UpdateGameState();
            gamePlayInGameCanvas.UpdateScore(score);
        }

        

        public void GameOver()
        {
            DebugLogHelper.Log("GameOver");

            StopGamePlayCoroutine();
            gameState = GAME_STATE.OVER;
            score = 0;

            player.GameOver();
            
            gamePlayOutGameCanvas.UpdateGameState();
            gamePlayInGameCanvas.UpdateScore(score);
        }

        public void StopGamePlayCoroutine()
        {
            if (gamePlayCoroutine != null)
                StopCoroutine(gamePlayCoroutine);
        }

        private IEnumerator CoGamePlaying()
        {
            while (true)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (gameState != GAME_STATE.PLAYING)
                        yield return null;

                    player?.Jump();
                }

                yield return null;
            }
        }

        public void OnPlayerTriggerEnter(Collider2D collision)
        {
            if (collision.TryGetComponent<WallObject>(out var wall))
            {
                player.ReverseHorizontalDirection();
                
                var showWallDir = CommonFunc.ConvertToPlayerMoveDirToWallDir(player.GetHorizontalDirection());
                spikeManager.ShowSpike(showWallDir);

                score++;
                gamePlayInGameCanvas.UpdateScore(score);

            }

            if (collision.TryGetComponent<SpikeObject>(out var spike))
            {
                GameOver();
            }
        }
    }
}
