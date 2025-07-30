using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Melon
{
    public class UITabList : GameElement
    {
        public GameObject attachObj;

        public List<UITabSlot> tabs = new List<UITabSlot>();

        private int selectTabIndex = -1;
        public Action<string> onClickTabAction = null;

        public void AttachTab(UITabSlot inTab)
        {
            if (tabs.Contains(inTab))
                return;
            
            inTab.transform.parent = attachObj.transform;
            tabs.Add(inTab);
            inTab.onClickAction = OnClickTab;
        }

        public void OnClickTab(UITabSlot inTab, bool isForce = false)
        {
            if (inTab == null)
                return;

            if (selectTabIndex == inTab.tabIndex && isForce == false)
                return;
            
            tabs.ForEach(item =>
            {
                item.SetTabSelect(item.tabIndex == inTab.tabIndex);
                if (item.tabIndex == inTab.tabIndex)
                {
                    selectTabIndex = item.tabIndex;
                    onClickTabAction?.Invoke(item.tabSlotData?.tabKey);
                }
            });
        }

        public void ResetTabList()
        {
            selectTabIndex = -1;
            tabs.ForEach(item => item.SetTabSelect(false));
        }

        public void ForceSelectTab(UITabSlotData inData)
        {
            ForceSelectTab(inData.tabKey);
        }
        
        public void ForceSelectTab(UITabSlot inTab)
        {
            ForceSelectTab(inTab.tabSlotData?.tabKey);
        }

        public void ForceSelectTab(string inTabKey)
        {
            ResetTabList();
            var tab = tabs.Find(item => item.tabSlotData?.tabKey == inTabKey);
            OnClickTab(tab, true);
        }

        public override void Init()
        {
            base.Init();
            ResetTabList();
        }
    }
}
