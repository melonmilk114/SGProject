using System;
using System.Collections.Generic;
using Melon;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace LuckyDefense
{
    public partial class BattleContent : Melon.Content, ITowerSelectMenu, ITowerCreate
    {
        public class ContentData
        {
            public long stageSn;
        }

        public BattleTowerManager battleTowerManager;
        public BattleMissileManager battleMissileManager;
        public BattleMonsterManager battleMonsterManager;
        public BattleStageManager battleStageManager;

        public BattleService battleService;
        
        public BattleContentInGameCanvas inGameView;
        public BattleContentOutGameCanvas outGameView;
        
        public LineRenderer selectTowerGroupLineRenderer;
        private TowerGroupObject _selectTowerGroup = null;
        private TowerSpotObject _selectTowerSpot = null;

        public long stageSn = 0;
        public float battleStartElapsedTime = 0f;
        public bool isBattlePlaying = false;

        public override void InitContent()
        {
            base.InitContent();
            
            battleTowerManager.InitContent();
            battleStageManager.InitContent();
            battleMissileManager.InitContent();
            battleMonsterManager.InitContent();

            battleService.InitService();
            
            inGameView?.InitCanvas(this);
            outGameView?.InitCanvas(this);
            
            // 라인 렌더러 비활성화
            selectTowerGroupLineRenderer.enabled = false;
            selectTowerGroupLineRenderer.positionCount = 0;
        }

        public override Enum GetContentType()
        {
            return ContentManager.ContentType.BATTLE;
        }

        private void Update()
        {
            if (isBattlePlaying == false)
                return;
            
            battleStartElapsedTime += Time.deltaTime;
            
            MissileLunch();
            ProgressWave();
            
            DestroyMonster();
            DestroyMissiles();
            
            battleMissileManager.UpdateContent(Time.deltaTime);
            battleMonsterManager.UpdateContent(Time.deltaTime);
            
        }

        public override void DoShowCheck(object inData, ActionResult inActionResult)
        {
            if (inData is ContentData contentData)
            {
                stageSn = contentData.stageSn;

                battleStageManager.InitContentManager(stageSn);
            }

            inActionResult.OnSuccess();
        }
        
        public override void DoPostShow(object inData = null, ActionResult inActionResult = null)
        {
            // MEMO : 게임 시작
            base.DoPostShow(inData, inActionResult);
            isBattlePlaying = true;
        }

        private void MissileLunch()
        {
            // 미사일 발사 준비가 된 타워 리스트를 갱신
            battleTowerManager.UpdateMissileLaunchReadyTowerList();
            
            // 미사일 발사 준비된 타워 
            var towerGroupList = battleTowerManager.FindMissileLaunchReadyTowerGroupList();
            if(towerGroupList.Count <= 0)
                return;
            
            towerGroupList.ForEach(inTowerGroup =>
            {
                // 미사일에 맞을 적 있나?
                var monster = battleMonsterManager.FindTargetsInRange(inTowerGroup);
                if (monster == null)
                    return;
                
                // 미사일 발사!
                var lunchTowerList = inTowerGroup.DoMissileLaunch(monster);
                
                lunchTowerList.ForEach(inTower =>
                {
                    var missileData = battleService.GetTowerMissileData(inTower.tableData?.sn);
                    var missileDamage = battleService.GetTowerMissileDamage(inTower.tableData?.sn, missileData);
                    battleMissileManager?.CreateMissile(inTower, monster, missileData, missileDamage);
                });
            });
        }

        private void ProgressWave()
        {
            // 지금 생성할 몬스터가 있는가?
            var instanceMonsterID = battleStageManager.FindInstanceMonsterID(battleStartElapsedTime);
            if (instanceMonsterID <= 0)
                return;
            
            battleMonsterManager?.CreateMonster(instanceMonsterID, inGameView.CreateMonsterHpBar());
        }

        private void DestroyMissiles()
        {
            // 제거 되어야 할 미사일들 삭제하기
            var destroyList = battleMissileManager.FindDestroyReadyMissiles();
            destroyList.ForEach(inItem =>
            {
                battleMissileManager?.MissileDestroy(inItem);
            });
            
            
        }

        private void DestroyMonster()
        {
            var deadMonsters = battleMonsterManager.FindDeadMonster();
            if (deadMonsters.Count <= 0)
                return;
            
            deadMonsters.ForEach(inItem =>
            {
                battleService.MonsterDeathReward(inItem.tableData.sn);
                battleMonsterManager?.MonsterDeath(inItem);
            });
        }

        private void CreateTower(long inTowerSn)
        {
            battleTowerManager?.CreateTower(inTowerSn);
        }
 

        #region TowerSelect
        
        public Vector3 currTouchPosition
        {
            get
            {
                Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                worldMousePosition.z = 0; // 2D 환경에서는 z축을 0으로 설정
                return worldMousePosition;
            }
        }

        public void SelectTower(TowerGroupObject inTowerGroup)
        {
            _selectTowerGroup = inTowerGroup;
            _selectTowerSpot = battleTowerManager.FindAttachTowerSpot(_selectTowerGroup);
            
            inGameView?.ShowTowerSelectMenu(_selectTowerGroup);
            _selectTowerGroup?.view.ShowAttackRange(true);
        }
        
        public void SelectTowerMove(TowerSpotObject inTowerSpot, Vector3 inStartPos, Vector3 inEndPos)
        {
            ShowTowerGroupLineRenderer(inStartPos, inEndPos);
            battleTowerManager?.ChangeTowerSpotRedBack(inTowerSpot);
            
            inGameView?.HideTowerSelectMenu();
            _selectTowerGroup?.view.ShowAttackRange(false);
        }

        public void DeselectTower()
        {
            HideTowerGroupLineRenderer();
            
            inGameView?.HideTowerSelectMenu();
            _selectTowerGroup?.view.ShowAttackRange(false);
                
            _selectTowerGroup = null;
            _selectTowerSpot = null;
        }

        public void ShowTowerGroupLineRenderer(Vector3 inStartPos, Vector3 inEndPos)
        {
            selectTowerGroupLineRenderer.enabled = true;
            selectTowerGroupLineRenderer.positionCount = 2;
            selectTowerGroupLineRenderer.SetPosition(0, inStartPos);
            selectTowerGroupLineRenderer.SetPosition(1, inEndPos);
            battleTowerManager?.ChangeAllTowerSpotWhiteBack();
        }
        
        public void HideTowerGroupLineRenderer()
        {
            selectTowerGroupLineRenderer.enabled = false;
            selectTowerGroupLineRenderer.positionCount = 0;
            battleTowerManager?.ChangeAllTowerSpotWhiteBack();
        }
        
        public void OnMouseDown()
        {
            // UI 위 클릭 → 콜라이더 클릭 무시
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            // 이전에 선택 된 타워 그룹이 있으면 선택 해제
            if (_selectTowerGroup != null)
            {
                DeselectTower();
            }
            
            var touchTowerGroup = battleTowerManager.FindTowerGroupByTouch(currTouchPosition);

            if (touchTowerGroup != null)
            {
                SelectTower(touchTowerGroup);
            }
            else
            {
                DeselectTower();
            }
        }
        public void OnMouseDrag()
        {
            // UI 위 클릭 → 콜라이더 클릭 무시
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            if (_selectTowerGroup != null)
            {
                var touchTowerSpot = battleTowerManager?.FindTowerSpotByTouch(currTouchPosition);

                if (touchTowerSpot == null)
                {
                    Debug.Log("터치가 영역을 벗어남");
                    return;
                }
                
                // 같은 타워 스팟이면 무시
                if (touchTowerSpot == _selectTowerSpot)
                {
                    // 라인 그리기 해제
                    HideTowerGroupLineRenderer();
                }
                else
                {
                    SelectTowerMove(touchTowerSpot, _selectTowerGroup.transform.position, touchTowerSpot.transform.position);
                }
            }
        }

        public void OnMouseUp()
        {
            // UI 위 클릭 → 콜라이더 클릭 무시
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            if (_selectTowerGroup != null)
            {
                // 타워 그룹이 이동이 가능한 위치인치 판단하여 타워 그룹을 이동 시킨다
                var touchTowerSpot = battleTowerManager?.FindTowerSpotByTouch(currTouchPosition);

                if (touchTowerSpot == null)
                {
                    DeselectTower();
                }
                else if (touchTowerSpot != _selectTowerSpot)
                {
                    // 이전에 선택 된 타워 그룹이 있던 spot에서 타워 그룹을 제거 한다.
                    _selectTowerSpot.DetachTowerGroup();
                    // 새로운 타워 스팟에 타워 그룹을 붙인다
                    touchTowerSpot.AttachTowerGroup(_selectTowerGroup, false);
                
                    // 타워 이동
                    _selectTowerGroup.AttachMove((inTowerGroup) =>
                    {
                        DeselectTower();
                    });
                    
                    HideTowerGroupLineRenderer();
                    Debug.Log(touchTowerSpot.name + " 선택됨");
                }
            }
        }

        #endregion

        #region ITowerSelectMenu
        
        public void SellTowerGroup(TowerGroupObject inTowerGroup)
        {
            battleService.RefundTowerCost(inTowerGroup.TowerTableData.sn);
            battleTowerManager?.TowerRemove(inTowerGroup);
        }
        
        public bool IsTowerMergeAvailable(TowerGroupObject inTowerGroup)
        {
            if (inTowerGroup.GetAttachTowerCount() < GameConfigData.maxTowerMergeCount)
            {
                DebugLogHelper.Log("합쳐진 타워 갯수가 부족함");
                return false;
            }
            
            // MEMO : 타워 업그레이드 체크
            if (battleService.IsTowerMergeAvailable(inTowerGroup.TowerTableData.sn) == false)
            {
                DebugLogHelper.LogError($"더이상 타워 업그레이드 할수 없습니다.");
                return false;
            }

            return true;
        }
        public void MergeTowerGroup(TowerGroupObject inTowerGroup)
        {
            if (IsTowerMergeAvailable(inTowerGroup) == false)
            {
                DebugLogHelper.LogError($"더이상 타워 업그레이드 할수 없습니다.");
                return;
            }

            
            var randomPickSn = battleService.GetTowerRandomSummon(inTowerGroup.TowerTableData.grade + 1);
            if(randomPickSn <= 0)
            {
                DebugLogHelper.LogError($"상위 타워를 소환 할 수 없습니다.");
                return;
            }
            
            battleTowerManager.TowerRemove(inTowerGroup);
            CreateTower(randomPickSn);

            DeselectTower();
            
            
        }
        
        #endregion

        #region ITowerCreate

        public bool IsCreateRandomTower()
        {
            // MEMO : 타워 생성 재화 체크
            if (battleService.IsTowerSummonCostEnough() == false)
            {
                DebugLogHelper.LogError($"타워 생성 재화가 부족 합니다.");
                return false;
            }
            
            return true;
        }
        public void CreateRandomTower()
        {
            if (IsCreateRandomTower() == false)
                return;

            var createSn = battleService.GetTowerRandomSummon(battleService.GetTowerSummonDefaultLevel());
            if (createSn <= 0)
            {
                DebugLogHelper.LogError("createSn is 0");
                return;
            }

            bool created = battleTowerManager.CreateTower(createSn);
            if(created)
                battleService.PayTowerSummonCost();
            
        }

        #endregion
        
    }
}
