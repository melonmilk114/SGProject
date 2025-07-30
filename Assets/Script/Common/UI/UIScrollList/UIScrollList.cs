using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Melon
{
    public class UIScrollList : GameElement
    {
        public ScrollRect scrollRect;
        public List<UIScrollListSlot> slots = new List<UIScrollListSlot>();

        public override void OnEnableFunc()
        {
            base.OnEnableFunc();
            ScrollPosReset();
        }

        public void ScrollPosReset()
        {
            if (scrollRect != null)
            {
                scrollRect.horizontalNormalizedPosition = scrollRect.horizontal ? 1 : 0;
                scrollRect.verticalNormalizedPosition = scrollRect.vertical ? 1 : 0;
            }
        }
        
        public GameObject GetListParent()
        {
            return scrollRect != null ? scrollRect.content.gameObject : gameObject;
        }

        private UIScrollListSlot GetHideSlot<T>() where T : GameElement
        {
            return slots.Find(inItem =>
            {
                if (inItem is T comp)
                    return inItem.gameObject.activeSelf == false && inItem.isEmpty;

                return false;
            });
        }

        public List<UIScrollListSlot> GetShowSlots()
        {
            return slots.FindAll(inItem => inItem.gameObject.activeSelf);
        }
        
        public void AllHideSlot()
        {
            slots?.ForEach(item => item.SlotHide());
        }

        public void HideSlot(int inIndex)
        {
            slots?.ForEach(item =>
            {
                if (item.slotIndex == inIndex)
                    item.SlotHide();
            });
        }

        public void AllHideSpawnSlots<T>(int inCount, T inTemplate, System.Action<T> inOnSpawn = null, System.Action inOnFinish = null) where T : GameElement
        {
            AllHideSlot();
            for (int idx = 0; idx < inCount; ++idx)
            {
                SpawnSlot(inTemplate, inOnSpawn);
            }

            inOnFinish?.Invoke();
        }

        public virtual void SpawnSlot<T>(T inTemplate, System.Action<T> inOnSpawn = null) where T : GameElement
        {
            UIScrollListSlot spawnItem = GetHideSlot<T>();
            if (spawnItem == null)
            {
                GameElement spawnItemObj = null;//Instantiate(inTemplate.gameObject, GetListParent());
                spawnItemObj = ObjectPoolManager.Instance.DequeuePool<T>(GetListParent());
                spawnItemObj.gameObject.SetActive(true);
                spawnItemObj.name = inTemplate.gameObject.name + "_" + slots.Count;
                spawnItemObj.transform.SetAsLastSibling();

                spawnItem = spawnItemObj.GetComponent<UIScrollListSlot>();
                spawnItem.slotIndex = slots.Count;
                spawnItem.isEmpty = false;
                slots.Add(spawnItem);
                inOnSpawn?.Invoke(spawnItemObj.GetComponent<T>());
            }
            else
            {
                spawnItem.transform.SetAsLastSibling();
                spawnItem.gameObject.SetActive(true);
                spawnItem.isEmpty = false;
                inOnSpawn?.Invoke(spawnItem.GetComponent<T>());
            }
        }
    }
}