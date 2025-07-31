using System.Collections.Generic;
using System.Linq;
using Melon;
using UnityEngine;

namespace DontTouchTheSpikes
{
    public class GamePlaySpikeManager : GameElement
    {
        public List<SpikeObject> spikeList_Left = new List<SpikeObject>();
        public List<SpikeObject> spikeList_Right = new List<SpikeObject>();
        public List<SpikeObject> spikeList_Top = new List<SpikeObject>();
        public List<SpikeObject> spikeList_Bottom = new List<SpikeObject>();
        
        public List<SpikeObject> GetSpikeList(WALL_DIR inWallDir = WALL_DIR.NONE)
        {
            return inWallDir switch
            {
                WALL_DIR.NONE => spikeList_Left
                    .Concat(spikeList_Right)
                    .Concat(spikeList_Top)
                    .Concat(spikeList_Bottom)
                    .ToList(),
                WALL_DIR.LEFT => spikeList_Left,
                WALL_DIR.RIGHT => spikeList_Right,
                WALL_DIR.TOP => spikeList_Top,
                WALL_DIR.BOTTOM => spikeList_Bottom,
                _ => new List<SpikeObject>()
            };
        }
        
        public void ShowSpike(WALL_DIR inWallDir, bool isImmediately = false)
        {
            List<SpikeObject> spikeList = GetSpikeList(inWallDir);
            spikeList.ForEach(inForItem =>
            {
                bool isShow = Random.Range(0, 2) == 1;
                if(isShow)
                    inForItem.ShowSpike(isImmediately);
                else
                    inForItem.HideSpike(isImmediately);
            });
        }
        
        public void AllShowSpike(bool isImmediately = false)
        {
            List<SpikeObject> spikeList = GetSpikeList();
            spikeList.ForEach(inForItem => inForItem.ShowSpike(isImmediately));
        }
        public void AllHideSpike(WALL_DIR inWallDir, bool isImmediately = false)
        {
            List<SpikeObject> spikeList = GetSpikeList(inWallDir);
            spikeList.ForEach(inForItem => inForItem.HideSpike(isImmediately));
        }
    }
}