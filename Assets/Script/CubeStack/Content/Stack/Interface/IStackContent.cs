namespace CubeStack
{
    public interface IStackContent
    {
        public void AddScore(int inScore);
        public void GameReady();
        public void GameStart();
        public void GameOver();
        
        public GAME_STATE GetGameState();
    }
}