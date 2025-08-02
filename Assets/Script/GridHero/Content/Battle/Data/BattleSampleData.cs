using System.Collections.Generic;
using UnityEngine;

namespace GridHero.Battle
{
    [System.Serializable]
    public class BattleSampleUnitData
    {
        public UnitObject unitPrefab;
        public Vector2Int createOffsetCoord;
    }
    [System.Serializable]
    public class BattleSampleTileData
    {
        public TileObject tilePrefab;
        public TileData tileData;
    }
    [System.Serializable]
    public class BattleSampleData
    {
        public int tmpTileRow;
        public int tmpTileColumn;
        public float tmpTileWidth;
        public float tmpTileHeight;
        
        public List<BattleSampleUnitData> tmpUnitDataList = new List<BattleSampleUnitData>();
        public List<BattleSampleTileData> tmpTileDataList = new List<BattleSampleTileData>();

        
    }
}