using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Melon
{
    public class UITabSlotData
    {
        public string selectTabImgPath = "";
        public string deselectTabImgPath = "";
        public string selectTabText = "";
        public string deselectTabText = "";
        public string tabKey = "";
        public int tabIndex = 0;
    }
    public class UITabSlot : GameElement
    {
        public GameElement selectObj;
        public UIImage selectBackImg;
        public UIText selectText;
        public GameElement deselectObj;
        public UIImage deselectBackImg;
        public UIText deselectText;
        public UIGameButton button;
        
        public int tabIndex = 0;
        public bool selectTab = false;
        public Action<UITabSlot, bool> onClickAction = null;

        public UITabSlotData tabSlotData = null;

        public override void OnAwakeFunc()
        {
            button.SetClickAction(OnTabClick);
        }

        #region Set

        public void SetTabSlotData(UITabSlotData inData)
        {
            if (inData == null)
                return;
            
            tabSlotData = inData;
            tabIndex = tabSlotData.tabIndex;
            
            SetSelectTabText(inData.selectTabText);
            SetDeselectTabText(inData.deselectTabText);

            SetSelectBackImgPath(inData.selectTabImgPath);
            SetDeselectBackImgPath(inData.deselectTabImgPath);
        }
        public void SetSelectTabText(string inStr)
        {
            selectText.SetText(inStr);
        }
        
        public void SetDeselectTabText(string inStr)
        {
            deselectText.SetText(inStr);
        }

        public void SetSelectBackImg(Sprite inSprite)
        {
            selectBackImg.SetImage(inSprite);
        }
        
        public void SetDeselectBackImg(Sprite inSprite)
        {
            deselectBackImg.SetImage(inSprite);
        }
        
        public void SetSelectBackImgPath(string inPath)
        {
            if(inPath != "")
                selectBackImg.SetImage(inPath);
        }
        
        public void SetDeselectBackImgPath(string inPath)
        {
            if(inPath != "")
                deselectBackImg.SetImage(inPath);
        }
        
        public void SetTabSelect(bool inSelect)
        {
            selectTab = inSelect;
            if (selectTab)
            {
                selectObj.DoShow();
                deselectObj.DoHide();
            }
            else
            {
                selectObj.DoHide();
                deselectObj.DoShow();
            }
        }

        #endregion
        
        
        

        public void OnTabClick()
        {
            onClickAction?.Invoke(this, false);
        }
        
    }
}
