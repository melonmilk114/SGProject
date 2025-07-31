using System;
using System.Collections.Generic;
using GridHeroes.Battle;
using GridHeroes.UI;
using UnityEngine;

namespace GridHeroes
{
    public class BattleContent : Melon.Content
    , IBattleContent
    {
        public BattleSkillManager battleSkillManager;
        public BattleStateManager battleStateManager;
        public BattleTileManager battleTileManager;
        public BattleUnitManager battleUnitManager;

        public Transform tileRoot;
        public Transform unitRoot;

        public BattleOutGameCanvas battleOutGameCanvas;
        public BattleInGameCanvas battleInGameCanvas;
        
        public BattleController battleController;

        public override void InitContent()
        {
            base.InitContent();

            battleStateManager.battleContent = this;
            battleStateManager.InitManager();
            
            battleSkillManager.battleContent = this;

            battleUnitManager.battleContent = this;

            battleTileManager.onTileSelect = OnClickTile;

            battleOutGameCanvas.uiBattleResult.onGameRetry = BattleStart;
        }
        
        public override Enum GetContentType()
        {
            return ContentManager.ContentType.BATTLE;
        }
        
        public override void DoContentStart(object inData)
        {
            BattleStart();
        }

        public void BattleStart()
        {
            CreateTiles();
            CreateUnits();
            
            battleUnitManager.BattleStart();
            battleSkillManager.BattleStart();
            battleStateManager.BattleStart();
            
            var allUnits = battleUnitManager.GetAllUnits();
            for (int idx = 0; idx < allUnits.Count; idx++)
            {
                var unit = allUnits[idx];
                battleStateManager.AddBattleStateDelegate(BATTLE_STATE.TURN_START, unit.Co_TurnStart);
                battleStateManager.AddBattleStateDelegate(BATTLE_STATE.TURN_END, unit.Co_TurnEnd);
                battleStateManager.AddBattleStateDelegate(BATTLE_STATE.ROUND_START, unit.Co_RoundStart);
                battleStateManager.AddBattleStateDelegate(BATTLE_STATE.ROUND_END, unit.Co_RoundEnd);
            }
            
            
            
            HideActionPanel();
            HideBattleResult();
        }
        
        public bool OnChangeUnitTileOffsetCoord(UnitObject inUnit, Vector2Int inTileOffsetCoord)
        {
            var findTile = battleTileManager.FindTileByCoord(inTileOffsetCoord);
            if (findTile == null) return false;
            
            inUnit.transform.position = findTile.transform.position;
            return true;
        }

        #region Create
        public void CreateTiles()
        {
            battleTileManager?.ClearTiles();

            // MEMO : 추후에 오브젝트풀로 변경할 예정
            GameObject tileSet = new GameObject("Tiles");
            tileSet.transform.SetParent(tileRoot.transform, false);

            int rowIdx = -1;
            int colIdx = -1;
            GameObject row = null;
            GameObject col = null;
            for (int idx = 0; idx < battleController.battleSampleData.tmpTileDataList.Count; idx++)
            {
                var data = battleController.battleSampleData.tmpTileDataList[idx];

                if (data.tileData.offsetCoord.y != rowIdx)
                {
                    rowIdx = data.tileData.offsetCoord.y;

                    row = new GameObject($"Row ({rowIdx})");
                    row.transform.SetParent(tileSet.transform, false);
                }
                
                colIdx = data.tileData.offsetCoord.x;
                Vector3 position = new Vector3(colIdx * battleController.battleSampleData.tmpTileWidth,
                    rowIdx * battleController.battleSampleData.tmpTileHeight, 0);

                if (rowIdx % 2 == 1)
                {
                    position += new Vector3(0.5f * battleController.battleSampleData.tmpTileWidth, 0, 0);
                }
                battleTileManager.CreateTile(data.tilePrefab, data.tileData, position, new Vector2Int(colIdx, rowIdx), row.transform);
            }
        }
        
        public void CreateUnits()
        {
            battleUnitManager?.ClearUnits();
            for (int idx = 0; idx < battleController?.battleSampleData.tmpUnitDataList.Count; idx++)
            {
                var createUnitData = battleController.battleSampleData.tmpUnitDataList[idx];
                var createUnit = battleUnitManager?.CreateUnit(createUnitData.unitPrefab, createUnitData.createOffsetCoord, unitRoot);
                battleStateManager.AddBattleStateDelegate(BATTLE_STATE.TURN_START, createUnit.Co_TurnStart);
                battleStateManager.AddBattleStateDelegate(BATTLE_STATE.TURN_END, createUnit.Co_TurnEnd);
                battleStateManager.AddBattleStateDelegate(BATTLE_STATE.ROUND_START, createUnit.Co_RoundStart);
                battleStateManager.AddBattleStateDelegate(BATTLE_STATE.ROUND_END, createUnit.Co_RoundEnd);
            }
        }

        #endregion
        
        public void OnClickTile(TileObject inTile)
        {
            CurrTurnSkill?.OnClickTargetTile(inTile);
        }

        public void ClearCurrTurn()
        {
            CurrTurnUnit = null;
            CurrTurnSkill = null;
        }

        public UnitObject CurrTurnUnit { get; set; }
        public UnitSkill CurrTurnSkill { get; set; }

        public TileObject FindTileByCoord(Vector2Int coord)
        {
            return battleTileManager.FindTileByCoord(coord);
        }

        public List<TileObject> GetAllTiles()
        {
            return battleTileManager.GetAllTiles();
        }

        public void ResetSkillTargetScore(int inScore = 0)
        {
            battleTileManager.ResetSkillTargetScore(inScore);
        }

        public void ChangeTileColor(TileObject inTile, Color color)
        {
            battleTileManager.ChangeTileColor(inTile, color);
        }

        public void ChangeTileColor(List<TileObject> inTiles, Color color)
        {
            battleTileManager.ChangeTileColor(inTiles, color);
        }

        public int GetManhattanDist(Vector2Int inFrom, Vector2Int inTo)
        {
            return battleTileManager.GetManhattanDist(inFrom, inTo);
        }

        public int GetManhattanDist(TileObject inFrom, TileObject inTo)
        {
            return battleTileManager.GetManhattanDist(inFrom, inTo);
        }

        public List<TileObject> GetAdjacentTiles(TileObject inTile)
        {
            return battleTileManager.GetAdjacentTiles(inTile);
        }

        public TileObject GetLeftTile(TileObject inTile)
        {
            return battleTileManager.GetLeftTile(inTile);
        }

        public TileObject GetRightTile(TileObject inTile)
        {
            return battleTileManager.GetRightTile(inTile);
        }

        public TileObject GetUpLeftTile(TileObject inTile)
        {
            return battleTileManager.GetUpLeftTile(inTile);
        }

        public TileObject GetUpRightTile(TileObject inTile)
        {
            return battleTileManager.GetUpRightTile(inTile);
        }

        public TileObject GetDownLeftTile(TileObject inTile)
        {
            return battleTileManager.GetDownLeftTile(inTile);
        }

        public TileObject GetDownRightTile(TileObject inTile)
        {
            return battleTileManager.GetDownRightTile(inTile);
        }

        public TileObject GetTileByUnit(UnitObject inUnitObject)
        {
            return battleTileManager.GetTileByUnit(inUnitObject);
        }

        public List<TileObject> GetTileByUnit(List<UnitObject> inUnitList)
        {
            return battleTileManager.GetTileByUnit(inUnitList);
        }

        public List<UnitObject> GetAllUnits()
        {
            return battleUnitManager.GetAllUnits();
        }
        public UnitObject FindUnitByCoord(Vector2Int inOffsetCoord)
        {
            return battleUnitManager.FindUnitByCoord(inOffsetCoord);
        }

        public UnitObject FindUnitByTile(TileObject inTile)
        {
            return battleUnitManager.FindUnitByTile(inTile);
        }

        public List<UnitObject> FindUnitByTile(List<TileObject> inTileList)
        {
            return battleUnitManager.FindUnitByTile(inTileList);
        }

        public List<UnitObject> GetUnits(UNIT_FACTION inFaction)
        {
            return battleUnitManager.GetUnits(inFaction);
        }

        public List<UnitObject> GetAdjacentUnits(UnitObject inUnit, UNIT_FACTION inFaction, int inDist = 1)
        {
            return battleUnitManager.GetAdjacentUnits(inUnit, inFaction, inDist);
        }

        public UnitObject FindNowTunUnit()
        {
            return battleUnitManager.FindNowTunUnit();
        }


        public List<UnitSkill> GetSkills(UnitObject inUnitObject)
        {
            return battleSkillManager.GetSkills(inUnitObject);
        }
        
        public List<UnitSkill> GetCanUseSkills(UnitObject inUnitObject)
        {
            return battleSkillManager.GetCanUseSkills(inUnitObject);
        }
        
        public List<UnitSkill> GetCanUseSkills(UnitObject inUnitObject, SKILL_TYPE inSkillType)
        {
            return battleSkillManager.GetCanUseSkills(inUnitObject, inSkillType);
        }
        
        public void UseSkill(UnitSkill inSkill)
        {
            battleSkillManager.UseSkill(inSkill);
        }
        
        
        // UI관련
        public UIHpBar CreateHpBar(UnitObject inUnit)
        {
            return battleInGameCanvas.CreateHpBar(inUnit);
        }
        
        public void SetTurnUnit(UnitObject inUnit)
        {
            battleInGameCanvas.SetTurnUnit(inUnit);
        }

        public void ShowActionPanel(UnitObject inUnit, Action<UnitSkill> inOnClickSkill)
        {
            battleOutGameCanvas.ShowActionPanel(inUnit, battleSkillManager.GetSkills(inUnit), inOnClickSkill);
        }

        public void HideActionPanel()
        {
            battleOutGameCanvas.ShowActionPanel(null, null, null);
        }
        
        public void ShowBattleResult(bool isInWin)
        {
            battleOutGameCanvas.ShowBattleResult(isInWin);
        }
        public void HideBattleResult()
        {
            battleOutGameCanvas.HideBattleResult();
        }
    }
}