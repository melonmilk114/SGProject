using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GridHero.Battle
{
    public class BattleUnitManager : MonoBehaviour
    {
        private List<UnitObject> _unitList = new List<UnitObject>();

        public IBattleContent battleContent;
        
        public Func<UnitObject, Vector2Int, bool> onChangeUnitTileOffsetCoord;
        public Func<Vector2Int, Vector2Int, int> onGetManhattanDist;

        public void BattleStart()
        {
            _unitList.ForEach(inForItem => inForItem.PlayIdleAnimation());
        }
        
        public void ClearUnits()
        {
            _unitList.ForEach(inForItem => Destroy(inForItem.gameObject));
            _unitList.Clear();
        }
        
        public UnitObject CreateUnit(UnitObject inOriginalUnit, Vector2Int inTileOffsetCoord, Transform inUnitRoot)
        {
            var newUnit = Instantiate(inOriginalUnit, inUnitRoot.transform);
            newUnit.battleContent = battleContent;
            newUnit.InitUnit(inTileOffsetCoord);
            battleContent.OnChangeUnitTileOffsetCoord(newUnit, inTileOffsetCoord);
            _unitList.Add(newUnit);

            return newUnit;
        }

        public UnitObject FindUnitByCoord(Vector2Int inOffsetCoord)
        {
            return _unitList.Find(inFindItem => inFindItem.tileOffsetCoord == inOffsetCoord);
        }
        public List<UnitObject> FindUnitByCoord(List<Vector2Int> inOffsetCoordList)
        {
            return _unitList
                .Where(unit => inOffsetCoordList.Contains(unit.tileOffsetCoord))
                .ToList();
        }

        public UnitObject FindUnitByTile(TileObject inTile)
        {
            return FindUnitByCoord(inTile.offsetCoord);
        }

        public List<UnitObject> FindUnitByTile(List<TileObject> inTileList)
        {
            var tileCoordList = inTileList.Select(tile => tile.offsetCoord).ToList();

            return FindUnitByCoord(tileCoordList);
        }

        public UnitObject FindUnitByCoord(int inX, int inY)
        {
            return FindUnitByCoord(new Vector2Int(inX, inY));
        }

        public UnitObject FindNowTunUnit()
        {
            return _unitList
                .Where(creature => creature.IsTurnAvailable())
                .OrderBy(creature => creature.GetStatValue(STAT_TYPE.INITIATIVE))
                .FirstOrDefault();
        }

        public List<UnitObject> GetAllUnits()
        {
            return _unitList;
        }
        
        public List<UnitObject> GetUnits(UNIT_FACTION inFaction)
        {
            return _unitList.Where(item => item.unitFaction == inFaction).ToList();
        }
        
        public List<UnitObject> GetAdjacentUnits(UnitObject inUnit, UNIT_FACTION inFaction, int inDist = 1)
        {
            return _unitList
                .Where(item => 
                    item != inUnit && 
                    item.unitFaction == inFaction &&
                    item.IsAlive() &&
                    battleContent.GetManhattanDist(inUnit.tileOffsetCoord, item.tileOffsetCoord) <= inDist
                )
                .ToList();
        }
    }
}