using GridHeroes.Battle;
using Melon;
using UnityEngine;

namespace GridHeroes
{
    public class BattleController : GameElement
    {
        public BattleSampleData battleSampleData = new BattleSampleData();
        
        
        
        [ContextMenu("Create Sample TileData")]
        public void CreateSampleTileData()
        {
            battleSampleData.tmpTileDataList.Clear();
            for (int row = 0; row < battleSampleData.tmpTileRow; row++)
            {
                for (int column = 0; column < battleSampleData.tmpTileColumn; column++)
                {
                    BattleSampleTileData tmpTileData = new BattleSampleTileData
                    {
                        tilePrefab = Resources.Load<TileObject>("GridHero/Prefab/TileObject"),
                        tileData = new TileData
                        {
                            offsetCoord = new Vector2Int(row, column),
                            tileType = TILE_TYPE.NORMAL,
                        },
                    };
                    battleSampleData.tmpTileDataList.Add(tmpTileData);
                }
            }
        }
    }
}