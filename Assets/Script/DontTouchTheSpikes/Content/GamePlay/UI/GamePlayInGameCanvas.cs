using Melon;

namespace DontTouchTheSpikes
{
    public class GamePlayInGameCanvas : GameElement
    {
        public UIGameLabel uiScoreLabel; 
        
        public void UpdateScore(int score)
        {
            if (uiScoreLabel != null)
            {
                uiScoreLabel.SetText(score.ToString());
            }
        }
        
        
    }
}