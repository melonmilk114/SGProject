using UnityEngine;

namespace Melon
{
    public class UIScrollListSlot : GameElement
    {
        public int slotIndex = -1;
        public bool isEmpty = true;

        public void SlotHide()
        {
            slotIndex = -1;
            isEmpty = true;
            gameObject.SetActive(false);
        }
    }
}