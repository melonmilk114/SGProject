using UnityEngine;

namespace GridHeroes.Battle
{
#if UNITY_EDITOR
    [System.Serializable]
#endif
    public class TileData
    {
        public Vector2Int offsetCoord = new Vector2Int();
        public TILE_TYPE tileType = TILE_TYPE.NORMAL;
    }
}