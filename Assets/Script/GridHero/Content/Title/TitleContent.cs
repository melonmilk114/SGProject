using System;
using Melon;

namespace GridHero
{
    public class TitleContent: Content
    {
        public UIGameLabel titleLabel;
        public UIGameButton startButton;
        public override void OnAwakeFunc()
        {
            base.OnAwakeFunc();
            titleLabel?.SetText("Grid Heros");
            startButton?.SetClickAction(() =>
            {
                GetContentManager(inMgr =>
                {
                    inMgr.DoShowContent(ContentManager.ContentType.LOADING);
                });
            });
        }
        
        public override Enum GetContentType()
        {
            return ContentManager.ContentType.TITLE;
        }
    }
}