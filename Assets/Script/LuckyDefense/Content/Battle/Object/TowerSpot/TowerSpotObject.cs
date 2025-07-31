using System;
using System.Collections.Generic;
using Melon;
using UnityEngine;
using UnityEngine.Serialization;

namespace LuckyDefense
{
    public class TowerSpotObject : GameElement
    {
        public TowerSpotView view;
        public TowerSpotData data;

        public TowerGroupObject towerGroup = null;

        public void Init(TowerSpotData inData)
        {
            data = inData;
        }

        public bool IsEmptySpot()
        {
            return towerGroup == null;
        }
        
        public void AttachTowerGroup(TowerGroupObject inTowerGroup, bool inResetPos = true)
        {
            towerGroup = inTowerGroup;
            towerGroup.transform.SetParent(transform);
            if(inResetPos)
                towerGroup.transform.localPosition = Vector3.zero;
            towerGroup.transform.localScale = Vector3.one;
        }

        public void DetachTowerGroup()
        {
            towerGroup = null;
        }
    }
}