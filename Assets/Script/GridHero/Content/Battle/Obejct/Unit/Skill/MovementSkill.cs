using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GridHeroes.Battle
{
    public class MovementSkill : UnitSkill
    {
        // public override bool IsRealUseSkill(UnitObject inCaster)
        // {
        //     caster = inCaster;
        //     var list = FindAvailableSkillTiles();
        //     return list.Count > 0;
        // }
        public override void UpdateSkillTargetScore()
        {
            // 자동으로 스킬이 발동 될때 실행되는 함수
            // // 스킬 사용 타일에서 가장 먼 타일을 기준으로 스킬 타겟 점수 설정
            // var casterTile = battleSkillManager?.FindTileByCoord(caster.tileOffsetCoord);
            // availableSkillTiles.ForEach(inForItem =>
            // {
            //     inForItem.skillTargetScore = 0;
            //     if (battleSkillManager != null)
            //         inForItem.skillTargetScore += battleSkillManager.GetManhattanDist(casterTile, inForItem);
            // });
            
            battleContent?.ResetSkillTargetScore(int.MaxValue);
            
            // 가장 근처 적으로 이동
            // 이동 가능한 타일중 적과 가까운 타일을 찾아 점수를 매긴다
            var findUnits = battleContent.GetUnits(caster.GetEnemyFaction());

            var tiles = FindTargetAvailableTiles();
            tiles.ForEach(inForItem_1 =>
            {
                int minDistValue = int.MaxValue; 
                findUnits.ForEach(inForItem_2 =>
                {
                    var enemyTile = battleContent.FindTileByCoord(inForItem_2.tileOffsetCoord);
                    if (enemyTile != null && inForItem_2.IsAlive())
                    {
                        var distValue = battleContent.GetManhattanDist(inForItem_1, enemyTile);
                        if (distValue == 0)
                            return;
                        if (minDistValue > distValue)
                            minDistValue = distValue;
                    }
                });
                inForItem_1.skillTargetScore -= minDistValue;
            });
        }

        public override List<TileObject> FindTargetAvailableTiles()
        {
            // 거리가 3칸인 이동 스킬
            int spd = caster.GetStatValue(STAT_TYPE.SPD);
            
            var casterTile = battleContent?.FindTileByCoord(caster.tileOffsetCoord);
            Dictionary<int, List<TileObject>> checkDict = new Dictionary<int, List<TileObject>>();
            checkDict.Add(0, new List<TileObject>() {casterTile});
            
            List<Vector2Int> visitedTiles = new List<Vector2Int>();
            visitedTiles.Add(casterTile.offsetCoord);
            
            List<TileObject> returnValue = new List<TileObject>();
            returnValue.Add(casterTile);
            
            int distanceValue = 0;
            while (true)
            {
                var checkList = checkDict.ContainsKey(distanceValue) ? checkDict[distanceValue] : new List<TileObject>();
                if (checkList.Count <= 0)
                    break;
                
                distanceValue++;

                for (int idx = 0; idx < checkList.Count; idx++)
                {
                    var current = checkList[idx];
                    var adjacents = battleContent?.GetAdjacentTiles(current);
                    
                    foreach (var adjacent in adjacents)
                    {
                        // 타일이 없는 공간 제외
                        if(adjacent == null)
                            continue;
                    
                        // 이미 방문한 타일 제외
                        if (visitedTiles.Contains(adjacent.offsetCoord))
                            continue;

                        // 이동 불가능한 타일 제외
                        if(CanMove(adjacent) == false)
                            continue;

                        checkDict.TryAdd(distanceValue, new List<TileObject>());
                        checkDict[distanceValue].Add(adjacent);
                        
                        visitedTiles.Add(adjacent.offsetCoord);
                        returnValue.Add(adjacent);
                    }
                }

                if (distanceValue >= spd)
                    break;
            }
            return returnValue;
        }
        

        public override List<TileObject> FindMoveTiles()
        {
            var fromTile = battleContent?.FindTileByCoord(caster.tileOffsetCoord);
            Queue<TileObject> checkQueue = new Queue<TileObject>();
            checkQueue.Enqueue(fromTile);

            List<Vector2Int> visitedTiles = new List<Vector2Int>();
            visitedTiles.Add(fromTile.offsetCoord);
            
            List<TileObject> returnValue = new List<TileObject>();
            returnValue.Add(fromTile);

            if (fromTile.offsetCoord == targetSelectTile.offsetCoord)
                return returnValue;
            
            
            List<KeyValuePair<TileObject, int>> fromToTileDistCheck = new List<KeyValuePair<TileObject, int>>();
            
            while (checkQueue.Count > 0)
            {
                var current = checkQueue.Dequeue();
                var adjacents = battleContent?.GetAdjacentTiles(current);
                
                fromToTileDistCheck.Clear();
                foreach (var adjacent in adjacents)
                {
                    // 타일이 없는 공간 제외
                    if(adjacent == null)
                        continue;
                    
                    // 이미 방문한 타일 제외
                    if (visitedTiles.Contains(adjacent.offsetCoord))
                        continue;

                    // 이동 불가능한 타일 제외
                    if(CanMove(adjacent) == false)
                        continue;
                    
                    visitedTiles.Add(adjacent.offsetCoord);
                    
                    // 도착지점 도착
                    if (adjacent.offsetCoord == targetSelectTile.offsetCoord)
                    {
                        fromToTileDistCheck.Clear();
                        fromToTileDistCheck.Add(new KeyValuePair<TileObject, int>(adjacent, battleContent.GetManhattanDist(adjacent, targetSelectTile)));
                        break;
                    }
                    
                    fromToTileDistCheck.Add(new KeyValuePair<TileObject, int>(adjacent, battleContent.GetManhattanDist(adjacent, targetSelectTile)));
                }

                if (fromToTileDistCheck.Count == 0)
                {
                    // 더 이상 이동할 수 있는 타일이 없으면 종료
                    break;
                }
                else
                {
                    var shortDist = fromToTileDistCheck.OrderBy(item => item.Value).First();
                    checkQueue.Enqueue(shortDist.Key);
                    returnValue.Add(shortDist.Key);
                    
                    if (shortDist.Key == targetSelectTile)
                        break;
                }
            }
            return returnValue;
        }

        public override IEnumerator Co_SkillExecuteRoutine()
        {
            int pathNodeCount = moveTiles.Count;
            if (pathNodeCount > 1)
            {
                caster.PlayLoopAnimation(UNIT_ANIM_TYPE.MOVE);
                for (int i = 0; i < pathNodeCount - 1; i++)
                {
                    if (moveTiles.Count <= 0)
                        break;
                    
                    yield return MoveFromToRoutine(moveTiles[i], moveTiles[i + 1]);
                    // if (_finalEffectTargets[i + 1].trap != null)
                    // {
                    //     yield return _finalEffectTargets[i + 1].trap.TrapRoutine();
                    //     break;
                    // }
                }
                caster.PlayIdleAnimation();
            }
        }
        
        IEnumerator MoveFromToRoutine(TileObject from, TileObject to)
        {
            caster.ChangeFacingDirection(to.transform.position);
            float elapsedTime = 0f;
            float duration = 0.3f;
            Vector3 fromPosition = from.transform.position;
            Vector3 toPosition = to.transform.position;
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / duration);
                caster.transform.position = Vector3.Lerp(fromPosition, toPosition, t);
                yield return null;
            }

            yield return to.OnTileStepped(caster);
            
            caster.ChangeAttachTileOffsetCoord(to);
        }

        public bool CanMove(TileObject tile)
        {
            if (battleContent.FindUnitByCoord(tile.offsetCoord) != null)
                return false;
            return true;
        }
        
        // public class MovementSkillCheckTile
        // {
        //     public TileObject tile;
        //     public TileObject prevTile;
        //     public int distance;
        // }
        //
        // public List<MovementSkillCheckTile> checkTiles = new List<MovementSkillCheckTile>();
        //
        // public override void UpdateSkillTargetScore()
        // {
        //     var casterTiler = tileFinderGetter?.FindTileByCoord(caster.tileOffsetCoord);
        //     tileFinderGetter?.GetAllTiles()?.ForEach(inForItem =>
        //     {
        //         inForItem.skillTargetScore = 0;
        //         if(tileFinderGetter != null)
        //             inForItem.skillTargetScore -= tileFinderGetter.GetManhattanDist(casterTiler, inForItem);
        //     });
        //     
        //     
        // }
        //
        // public override List<TileObject> FindAvailableSkillTiles(UnitObject inCasterUnit)
        // {
        //     int spd = 3;
        //     
        //     checkTiles.Clear();
        //
        //     var casterTile = tileFinderGetter?.FindTileByCoord(inCasterUnit.tileOffsetCoord);
        //     List<TileObject> returnValue = new List<TileObject>();
        //     returnValue.Add(casterTile);
        //     
        //     Queue<MovementSkillCheckTile> checkQueue = new Queue<MovementSkillCheckTile>();
        //     MovementSkillCheckTile casterCheckTile = new MovementSkillCheckTile
        //     {
        //         tile = casterTile,
        //         prevTile = null,
        //         distance = 0,
        //     };
        //     checkTiles.Add(casterCheckTile);
        //     checkQueue.Enqueue(casterCheckTile);
        //
        //     int searchCount = 0;
        //
        //     while (checkQueue.Count > 0 && searchCount < spd)
        //     {
        //         int checkQueueCount = checkQueue.Count;
        //         for (int i = 0; i < checkQueueCount; i++)
        //         {
        //             var current = checkQueue.Dequeue();
        //             var adjacents = tileFinderGetter?.GetAdjacentTiles(current.tile);
        //
        //             foreach (var adjacent in adjacents)
        //             {
        //                 if (adjacent == null || checkTiles.Any(inFindItem => inFindItem.tile == adjacent))
        //                     continue;
        //
        //                 if (CanMove(adjacent))
        //                 {
        //                     MovementSkillCheckTile checkTile = new MovementSkillCheckTile
        //                     {
        //                         tile = adjacent,
        //                         prevTile = current.tile,
        //                         distance = current.distance + 1,
        //                     };
        //                     
        //                     checkTiles.Add(checkTile);
        //                     checkQueue.Enqueue(checkTile);
        //                     
        //                     returnValue.Add(adjacent);
        //                 }
        //
        //                 bool CanMove(TileObject tile)
        //                 {
        //                     if (unitFinderGetter.FindUnitByCoord(tile.offsetCoord) != null)
        //                         return false;
        //                     return true;
        //                 }
        //             }
        //         }
        //
        //         searchCount++;
        //     }
        //
        //     return returnValue;
        // }
        //
        // public override List<TileObject> FindSkillUsePathTiles(TileObject inFromTile, TileObject inToTile)
        // {
        //     List<TileObject> path = new List<TileObject>();
        //
        //     MovementSkillCheckTile current = checkTiles.Find(inFindItem => inFindItem.tile == inToTile);
        //
        //     while (current.tile != inFromTile)
        //     {
        //         path.Add(current.tile);
        //         current = checkTiles.Find(inFindItem => inFindItem.tile == current.prevTile);
        //     }
        //
        //     path.Add(inFromTile);
        //     path.Reverse();
        //     return path;
        // }
        //
        // public override IEnumerator Co_SkillExecuteRoutine()
        // {
        //     int pathNodeCount = finalSkillPath.Count;
        //
        //     if (pathNodeCount > 1)
        //     {
        //         caster.PlayLoopAnimation(UNIT_ANIM_TYPE.MOVE);
        //         for (int i = 0; i < pathNodeCount - 1; i++)
        //         {
        //             yield return MoveFromToRoutine(finalSkillPath[i], finalSkillPath[i + 1]);
        //             // if (_finalEffectTargets[i + 1].trap != null)
        //             // {
        //             //     yield return _finalEffectTargets[i + 1].trap.TrapRoutine();
        //             //     break;
        //             // }
        //         }
        //
        //         caster.PlayIdleAnimation();
        //     }
        // }
        //
        // IEnumerator MoveFromToRoutine(TileObject from, TileObject to)
        // {
        //     caster.ChangeFacingDirection(to.transform.position);
        //
        //     float elapsedTime = 0f;
        //     float duration = 0.3f;
        //
        //     Vector3 fromPosition = from.transform.position;
        //     Vector3 toPosition = to.transform.position;
        //
        //     while (elapsedTime < duration)
        //     {
        //         elapsedTime += Time.deltaTime;
        //         float t = Mathf.Clamp01(elapsedTime / duration);
        //         caster.transform.position = Vector3.Lerp(fromPosition, toPosition, t);
        //         yield return null;
        //     }
        //
        //     caster.ChangeAttachTileOffsetCoord(to);
        // }
        
    }
}