using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Melon
{
    public class ContentManager : Manager
    {
        public List<Content> contents = new List<Content>();
        
        public ActionResult contentChangeActionResult;
        
        public override void InitManager()
        {
            base.InitManager();
            contents.AddRange(rootObj.GetComponentsInChildren<Content>(true));
            contents.ForEach(item =>
            {
                item.DoHide();
                item.InitContent();
            }); 
            
        }
        public Content FindContent(Enum inEnum)
        {
            return contents.Find(item => item.GetContentType().Equals(inEnum));
        }
        public virtual void DoShowContent(Enum inEnum, object inData = null, ActionResult inActionResult = null)
        {
            var showContent = FindContent(inEnum);
            if (showContent == null)
            {
                ContentChangeFail("");
                return;
            }

            contentChangeActionResult = inActionResult;
            
            showContent.DoShow(inData, new ActionResult()
            {
                onSuccess = () =>
                {
                    // 현재 보여주는거 제외 하고 전부 HIDE
                    ForceAllHideController(showContent.GetContentType());

                    ContentChangeSuccess();

                    showContent.DoContentStart(inData);
                },
                onFail = (err) =>
                {
                    Debug.LogError(err);
                    ContentChangeFail(err);
                }
            });
        }
        
        public virtual void DoHideContent(Enum inEnum, object inData = null, ActionResult inActionResult = null)
        {
            var hideContent = FindContent(inEnum);
            if (hideContent == null)
            {
                ContentChangeFail("");
                return;
            }
            
            contentChangeActionResult = inActionResult;
            
            hideContent.DoHide(inData, new ActionResult()
            {
                onSuccess = () =>
                {
                    ContentChangeSuccess();
                },
                onFail = (err) =>
                {
                    Debug.LogError(err);
                    ContentChangeFail(err);
                }
            });
        }
        
        public void ForceAllHideController(Enum inExcludeEnum = null)
        {
            contents.ForEach(item =>
            {
                if (inExcludeEnum != null && item.GetContentType().Equals(inExcludeEnum))
                    return;
                
                item.DoHide(null, null);
            });
        }
        
        public void ForceMoveTitleView()
        {
            
        }

        public void ContentChangeSuccess()
        {
            contentChangeActionResult?.OnSuccess();
        }
        public void ContentChangeFail(string inErr)
        {
            contentChangeActionResult?.OnFail(inErr);
        }
    }
}