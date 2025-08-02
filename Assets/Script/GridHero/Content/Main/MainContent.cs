using System;
using Melon;
using UnityEngine;

namespace GridHero
{
    public class MainContent : Content
    {
        public UIGameLabel mainLabel;
        public UIGameLabelButton battleButton;
        public override void OnAwakeFunc()
        {
            base.OnAwakeFunc();
            mainLabel?.SetText("Main Content");
            battleButton?.SetClickAction(() =>
            {
                GetContentManager(inMgr =>
                {
                    inMgr.DoShowContent(ContentManager.ContentType.BATTLE);
                });
            });
        }
        public override Enum GetContentType()
        {
            return ContentManager.ContentType.MAIN;
        }
    }
}