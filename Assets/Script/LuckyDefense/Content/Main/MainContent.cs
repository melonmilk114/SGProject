using System;
using System.Collections;
using Melon;
using UnityEngine;

namespace LuckyDefense
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
                    inMgr.DoShowContent(ContentManager.ContentType.BATTLE, new BattleContent.ContentData()
                    {
                        stageSn = 1,
                    });
                });
            });
        }
        public override Enum GetContentType()
        {
            return ContentManager.ContentType.MAIN;
        }
    }
}