using System;
using Melon;

namespace LuckyDefense
{
    public class TitleContent : Content
    {
        public UIGameLabel titleLabel;
        public UIGameButton startButton;
        public override void OnAwakeFunc()
        {
            base.OnAwakeFunc();
            titleLabel?.SetText("Lucky Defense");
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