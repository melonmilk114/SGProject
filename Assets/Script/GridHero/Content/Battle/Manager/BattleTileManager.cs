using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GridHero.Battle
{
    public class BattleTileManager : MonoBehaviour
    , ITileSelect
    {
        public TileObject tilePrefab;
        private Dictionary<Vector2Int, TileObject> _tiles = new Dictionary<Vector2Int, TileObject>();
        
        public Action<TileObject> onTileSelect;

        #region Finder & Getter

        public List<TileObject> GetAllTiles()
        {
            return _tiles.Values.ToList();
        }
        
        public TileObject FindTileByCoord(Vector2Int coord)
        {
            if (_tiles.TryGetValue(coord, out TileObject tile))
            {
                return tile;
            }
            return null;
        }
        
        public TileObject FindTileByCoord(int inX, int inY)
        {
            return FindTileByCoord(new Vector2Int(inX, inY));
        }

        public TileObject GetLeftTile(TileObject inTile) { return FindTileByCoord(inTile.LeftOffsetPos); }
        public TileObject GetRightTile(TileObject inTile) { return FindTileByCoord(inTile.RightOffsetPos); }
        public TileObject GetUpLeftTile(TileObject inTile) { return FindTileByCoord(inTile.UpLeftOffsetPos); }
        public TileObject GetUpRightTile(TileObject inTile) { return FindTileByCoord(inTile.UpRightOffsetPos); }
        public TileObject GetDownLeftTile(TileObject inTile) { return FindTileByCoord(inTile.DownLeftOffsetPos); }
        public TileObject GetDownRightTile(TileObject inTile) { return FindTileByCoord(inTile.DownRightOffsetPos); }
        public List<TileObject> GetAdjacentTiles(TileObject inTile)
        {
            return new List<TileObject>()
            {
                GetLeftTile(inTile),
                GetRightTile(inTile),
                GetUpLeftTile(inTile),
                GetUpRightTile(inTile),
                GetDownLeftTile(inTile),
                GetDownRightTile(inTile)
            };
        }
        
        public TileObject GetTileByUnit(UnitObject inUnitObject)
        {
            return FindTileByCoord(inUnitObject.tileOffsetCoord);
        }
        
        public List<TileObject> GetTileByUnit(List<UnitObject> inUnitList)
        {
            return inUnitList
                .Select(unit => GetTileByUnit(unit))
                .Where(tile => tile != null)
                .ToList();
        }

        #endregion

        public void ClearTiles()
        {
            foreach (var tile in _tiles.Values)
            {
                Destroy(tile.gameObject);
            }
            _tiles.Clear();
        }
        
        public void CreateTile(TileObject inTile, TileData inData, Vector3 inPos, Vector2Int inOffSetCoord, Transform inParent)
        {
            TileObject tile = Instantiate(inTile, inPos, Quaternion.identity);
            tile.transform.SetParent(inParent.transform, false);
            tile.SetData(inData, this, inOffSetCoord);
            tile.gameObject.name = $"Tile ({inOffSetCoord.x},{inOffSetCoord.y})";

            _tiles.Add(tile.offsetCoord, tile);
        }
        
        public void ResetSkillTargetScore(int inScore)
        {
            GetAllTiles().ForEach(inForItem => inForItem.skillTargetScore = inScore);
        }
        
        public int GetManhattanDist(Vector2Int inFrom, Vector2Int inTo)
        {
            var fromTile = FindTileByCoord(inFrom);
            var toTile = FindTileByCoord(inTo);

            return GetManhattanDist(fromTile, toTile);
        }
        
        public int GetManhattanDist(TileObject inFrom, TileObject inTo)
        {
            if (inFrom == null || inTo == null)
                return 0;
            
            int xDelta = Mathf.Abs(inFrom.cubeCoord.x-inTo.cubeCoord.x);
            int yDelta = Mathf.Abs(inFrom.cubeCoord.y-inTo.cubeCoord.y);
            int zDelta = Mathf.Abs(inFrom.cubeCoord.z-inTo.cubeCoord.z);

            return (xDelta + yDelta + zDelta) / 2;
        }

        public void OnTileSelect(TileObject inTile)
        {
            DebugLogHelper.Log($"selected at offset {inTile.offsetCoord} and cube {inTile.cubeCoord}");
            
            onTileSelect?.Invoke(inTile);
        }

        public void ChangeTileColor(TileObject inTile, Color color)
        {
            inTile.tileSprite.color = color;
        }

        public void ChangeTileColor(List<TileObject> inTiles, Color color)
        {
            inTiles.ForEach(inForItem => ChangeTileColor(inForItem, color));
        }
    }
}