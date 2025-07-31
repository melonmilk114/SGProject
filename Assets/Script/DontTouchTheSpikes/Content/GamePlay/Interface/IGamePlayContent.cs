using System.Collections.Generic;
using UnityEngine;

namespace DontTouchTheSpikes
{
    public interface IGamePlayContent
    {
        public void GameReady();
        public void GameStart();
        
        public GAME_STATE GetGameState();
        
    }
}