using System;
using System.Collections.Generic;
using Melon;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace LuckyDefense
{
    public partial class BattleContent : Content, ITowerSelectMenu, ITowerCreate, IBattleOutGameMenu
        , IObserver<BattleInfoData>
    {
        public class ContentData
        {
            public long stageSn;
        }

        // 타워를 관리하는 매니저
        public BattleTowerManager battleTowerManager;
        // 미사일을 관리하는 매니저
        public BattleMissileManager battleMissileManager;
        // 몬스터를 관리하는 매니저
        public BattleMonsterManager battleMonsterManager;
        // 웨이브를 관리하는 매니저
        public BattleStageManager battleStageManager;

        // 전투 관련 데이터를 관리하는 서비스
        public BattleService battleService;
        
        // 인게임 UI
        public BattleContentInGameCanvas inGameView;
        // 아웃게임 UI
        public BattleContentOutGameCanvas outGameView;
        
        // 타워 그룹 선택시 라인 렌더러
        public LineRenderer selectTowerGroupLineRenderer;
        // 선택 된 타워 그룹, 타워 스팟
        private TowerGroupObject _selectTowerGroup = null;
        private TowerSpotObject _selectTowerSpot = null;

        public long stageSn = 0;
        public float battleStartElapsedTime = 0f;
        public bool isBattlePlaying = false;

        // 컨텐츠 초기화
        public override void InitContent()
        {
            base.InitContent();
            
            battleTowerManager.InitContent();
            battleStageManager.InitContent();
            battleMissileManager.InitContent();
            battleMonsterManager.InitContent();

            battleService.InitService();
            
            inGameView?.InitCanvas(this);
            outGameView?.InitCanvas(this, this);
            
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

        // 컨텐츠가 화면에 보여지기 전에 호출하여 데이터 초기화
        public override void DoShowCheck(object inData, ActionResult inActionResult)
        {
            if (inData is ContentData contentData)
            {
                stageSn = contentData.stageSn;

                
            }

            inActionResult.OnSuccess();
        }
        
        public override void DoPostShow(object inData = null, ActionResult inActionResult = null)
        {
            // MEMO : 게임 시작
            base.DoPostShow(inData, inActionResult);
            isBattlePlaying = true;
            battleStartElapsedTime = 0;

            battleService.GameStart();
            battleMonsterManager.GameStart();
            battleMissileManager.GameStart();
            battleTowerManager.GameStart();
            battleStageManager.GameStart(stageSn);
            outGameView.HideGameConditionPopup();
        }

        /// <summary>
        /// 미사일 발사
        /// </summary>
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
                    var missileDamage = battleService.GetTowerMissileDamage(inTower);
                    battleMissileManager?.CreateMissile(inTower, monster, missileData, missileDamage);
                });
            });
        }

        /// <summary>
        /// 웨이브 진행
        /// </summary>
        private void ProgressWave()
        {
            // 지금 생성할 몬스터가 있는가?
            var instanceMonsterID = battleStageManager.FindInstanceMonsterID(battleStartElapsedTime);
            if (instanceMonsterID <= 0)
                return;
            
            battleMonsterManager?.CreateMonster(instanceMonsterID, inGameView.CreateMonsterHpBar());
            battleService.SetNowMonsterCount(battleMonsterManager.GetMonsterCount());
        }

        /// <summary>
        /// 제거되어야할 미사일들 제거
        /// </summary>
        private void DestroyMissiles()
        {
            // 제거 되어야 할 미사일들 삭제하기
            var destroyList = battleMissileManager.FindDestroyReadyMissiles();
            destroyList.ForEach(inItem =>
            {
                battleMissileManager?.MissileDestroy(inItem);
            });
            
            
        }

        /// <summary>
        /// 제거 되어야할 몬스터들 제거
        /// </summary>
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

            battleService.SetNowMonsterCount(battleMonsterManager.GetMonsterCount());
        }

        /// <summary>
        /// 타워 생성
        /// </summary>
        /// <param name="inTowerSn">생성할 타워 SN</param>
        private bool CreateTower(long inTowerSn)
        {
            return battleTowerManager.CreateTower(inTowerSn, battleService.GetTowerLevel(inTowerSn));
        }
 
        // 타워 선택 관련 
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

        /// <summary>
        /// 타워 그룹 선택
        /// </summary>
        /// <param name="inTowerGroup"></param>
        public void SelectTower(TowerGroupObject inTowerGroup)
        {
            // 선택한 타워 그룹 및 스팟 저장
            _selectTowerGroup = inTowerGroup;
            _selectTowerSpot = battleTowerManager.FindAttachTowerSpot(_selectTowerGroup);
            
            // 타워 선택 메뉴 표시
            inGameView?.ShowTowerSelectMenu(_selectTowerGroup);
            // 타워 공격 범위 표시
            _selectTowerGroup?.view.ShowAttackRange(true);
        }
        
        /// <summary>
        /// 선택한 타워 이동
        /// </summary>
        /// <param name="inTowerSpot"> 이동할 타워 스팟 </param>
        /// <param name="inStartPos"></param>
        /// <param name="inEndPos"></param>
        public void SelectTowerMove(TowerSpotObject inTowerSpot, Vector3 inStartPos, Vector3 inEndPos)
        {
            // 이동할 타워 스팟에 타워가 있는지 판단
            if (inTowerSpot.IsEmptySpot() == false)
            {
                var color = new Color(1,0,0,0.4f);
                // 라인 렌더러 표시
                ShowTowerGroupLineRenderer(inStartPos, inEndPos, color);
                battleTowerManager?.ChangeTowerSpotBackColor(inTowerSpot, color);
            }
            else
            {
                var color = new Color(1,1,0,0.4f);
                // 라인 렌더러 표시
                ShowTowerGroupLineRenderer(inStartPos, inEndPos, color);
                // 이동할 타워 스팟 표시
                battleTowerManager?.ChangeTowerSpotBackColor(inTowerSpot, color);
            }
            
            
            // 타워 선택 메뉴 숨김
            inGameView?.HideTowerSelectMenu();
            // 타워 공격 범위 숨김
            _selectTowerGroup?.view.ShowAttackRange(false);
        }

        /// <summary>
        /// 타워 선택 해제
        /// </summary>
        public void DeselectTower()
        {
            // 라인 렌더러 숨김
            HideTowerGroupLineRenderer();
            
            // 타워 선택 메뉴 숨김
            inGameView?.HideTowerSelectMenu();
            // 타워 공격 범위 숨김
            _selectTowerGroup?.view.ShowAttackRange(false);
            
            // 선택한 타워 그룹 및 스팟 초기화
            _selectTowerGroup = null;
            _selectTowerSpot = null;
        }

        /// <summary>
        /// 라인 랜더러 표시
        /// </summary>
        /// <param name="inStartPos"></param>
        /// <param name="inEndPos"></param>
        public void ShowTowerGroupLineRenderer(Vector3 inStartPos, Vector3 inEndPos, Color inColor)
        {
            selectTowerGroupLineRenderer.enabled = true;
            selectTowerGroupLineRenderer.positionCount = 2;
            selectTowerGroupLineRenderer.startColor = inColor;
            selectTowerGroupLineRenderer.endColor = inColor;
            selectTowerGroupLineRenderer.SetPosition(0, inStartPos);
            selectTowerGroupLineRenderer.SetPosition(1, inEndPos);
            battleTowerManager?.ChangeAllTowerSpotBackColor(new Color(0, 0, 0, 0));
        }
        
        /// <summary>
        /// 라인 랜더러 숨김
        /// </summary>
        public void HideTowerGroupLineRenderer()
        {
            selectTowerGroupLineRenderer.enabled = false;
            selectTowerGroupLineRenderer.positionCount = 0;
            battleTowerManager?.ChangeAllTowerSpotBackColor(new Color(0, 0, 0, 0));
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
                return;
            }
            
            // 터치한 위치에 타워 그룹이 있는지 확인
            var touchTowerGroup = battleTowerManager.FindTowerGroupByTouch(currTouchPosition);

            if (touchTowerGroup != null)
            {
                // 타워 그룹 선택
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
                // 터치한 위치에 타워 스팟이 있는지 확인
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
                    // 타워 그룹 이동
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
                    // 이동할 타워 스팟에 타워가 있는지 판단
                    if (touchTowerSpot.IsEmptySpot() == false)
                    {
                        DeselectTower();
                    }
                    else
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
        }

        #endregion

        // 타워 선택 메뉴 관련
        #region ITowerSelectMenu
        
        /// <summary>
        /// 타워 판매
        /// </summary>
        /// <param name="inTowerGroup"></param>
        public void SellTowerGroup(TowerGroupObject inTowerGroup)
        {
            // 타워 판매시 재화 획득
            battleService.RefundTowerCost(inTowerGroup.TowerTableData.sn);
            // 타워 제거
            battleTowerManager?.TowerRemove(inTowerGroup);
        }
        
        /// <summary>
        /// 타워 합성이 가능한지 체크
        /// </summary>
        /// <param name="inTowerGroup"></param>
        /// <returns></returns>
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
        
        /// <summary>
        /// 타워 합성 
        /// </summary>
        /// <param name="inTowerGroup"></param>
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

        // 타워 생성 관련
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
        /// <summary>
        /// 랜덤 타워 생성
        /// </summary>
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

            bool created = CreateTower(createSn);
            if(created)
                battleService.PayTowerSummonCost();
            
        }

        #endregion
        
        #region IBattleOutGameMenu
        public void OnClickGameExit()
        {
            GetContentManager(inMgr =>
            {
                inMgr.DoShowContent(ContentManager.ContentType.TITLE);
            });
        }
        #endregion

        #region OnNotify

        public void OnNotify(BattleInfoData inData_1)
        {
            // 승패 처리
            if (inData_1.nowMonsterCount >= inData_1.maxMonsterCount)
            {
                // 패배
                outGameView.ShowGameConditionPopup("LOSE");
                isBattlePlaying = false;
            }

            if (inData_1.nowMonsterCount <= 0 && battleStageManager.IsNextWaveAvailable() == false)
            {
                outGameView.ShowGameConditionPopup("WIN");
                isBattlePlaying = false;
            }
        }

        #endregion


        
    }
}
