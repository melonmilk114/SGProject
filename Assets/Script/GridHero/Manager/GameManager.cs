using UnityEngine;

namespace GridHero
{
    public class GameManager : Melon.GameManager
    {
        public override void InitManager()
        {
            base.InitManager();
        }
        
        public override void GameStart(object inData = null)
        {
            // 게임 시작
            
            contentManager.DoShowContent(ContentManager.ContentType.TITLE);
            
        }

        public override void GameEnd(object inData = null)
        {
        
        }
    }
}