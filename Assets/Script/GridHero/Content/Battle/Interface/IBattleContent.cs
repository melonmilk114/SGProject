using System;
using System.Collections.Generic;
using UnityEngine;

namespace GridHeroes.Battle
{
    public interface IBattleContent
    {
        // 공통
        public void ClearCurrTurn();
        public UnitObject CurrTurnUnit { get; set; }
        public UnitSkill CurrTurnSkill { get; set; }
        
        // 타일 관련
        public TileObject FindTileByCoord(Vector2Int coord);
        public List<TileObject> GetAllTiles();
        public void ResetSkillTargetScore(int inScore = 0);
        public void ChangeTileColor(TileObject inTile, Color color);

        public void ChangeTileColor(List<TileObject> inTiles, Color color);
        public int GetManhattanDist(Vector2Int inFrom, Vector2Int inTo);
        public int GetManhattanDist(TileObject inFrom, TileObject inTo);
        public List<TileObject> GetAdjacentTiles(TileObject inTile);
        public TileObject GetLeftTile(TileObject inTile);
        public TileObject GetRightTile(TileObject inTile);
        public TileObject GetUpLeftTile(TileObject inTile);
        public TileObject GetUpRightTile(TileObject inTile);
        public TileObject GetDownLeftTile(TileObject inTile);
        public TileObject GetDownRightTile(TileObject inTile);
        public TileObject GetTileByUnit(UnitObject inUnitObject);
        public List<TileObject> GetTileByUnit(List<UnitObject> inUnitList);

        public bool OnChangeUnitTileOffsetCoord(UnitObject inUnit, Vector2Int inTileOffsetCoord);
        
        
        // 유닛 관련
        public List<UnitObject> GetAllUnits();
        public UnitObject FindUnitByCoord(Vector2Int inOffsetCoord);
        public UnitObject FindUnitByTile(TileObject inTile);
        public List<UnitObject> FindUnitByTile(List<TileObject> inTileList);
        public List<UnitObject> GetUnits(UNIT_FACTION inFaction);
        public List<UnitObject> GetAdjacentUnits(UnitObject inUnit, UNIT_FACTION inFaction, int inDist = 1);
        
        public UnitObject FindNowTunUnit();
        
        // 스킬 관련
        public List<UnitSkill> GetSkills(UnitObject inUnitObject);
        public List<UnitSkill> GetCanUseSkills(UnitObject inUnitObject);
        public List<UnitSkill> GetCanUseSkills(UnitObject inUnitObject, SKILL_TYPE inSkillType);
        public void UseSkill(UnitSkill inSkill);
        
        // UI 관련
        public UIHpBar CreateHpBar(UnitObject inUnit);
        public void SetTurnUnit(UnitObject inUnit);
        public void ShowActionPanel(UnitObject inUnit, Action<UnitSkill> inOnClickSkill);
        public void HideActionPanel();
        public void ShowBattleResult(bool isInWin);
        public void HideBattleResult();
    }
}