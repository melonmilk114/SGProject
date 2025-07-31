using System;
using System.Collections.Generic;
using LuckyDefense.Interface;
using Melon;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LuckyDefense
{
    public class BattleTowerManager : GameElement, ITowerSelectMenu, ITowerCreate
    , ITargetObjectReceiver<TableDataManager>
    {
        [SerializeField] private List<TowerSpotObject> _towerSpots = new List<TowerSpotObject>();
        [SerializeField] private List<TowerGroupObject> _towerGroups = new List<TowerGroupObject>();
        
        public LineRenderer lineRenderer;
        
        public GameObject towerSpotsParent;
        
        public IBattleService battleService = null;

        public Action<TowerGroupObject> onCreateMissile = null;
        public Action<TowerGroupObject> onOpenTowerSelectMenu = null;
        public Action onCloseTowerSelectMenu = null;
        
        public TowerTableData towerTableData;
        
        #region ITargetObjectReceiver<TableDataManager>
        TableDataManager ITargetObjectReceiver<TableDataManager>.GetTargetObject { get; set; }
        public void SetTargetObject(TableDataManager inObject)
        {
            ((ITargetObjectReceiver<TableDataManager>) this).GetTargetObject = inObject;
        }
        
        public TableDataManager GetTableDataManager(System.Action<TableDataManager> inOnNotNull = null)
        {
            var returnManager = ((ITargetObjectReceiver<TableDataManager>) this).GetTargetObject;
            if (inOnNotNull != null)
                inOnNotNull.Invoke(returnManager);

            return returnManager;
        }
        #endregion
        
        #region FindFunc

        private TowerSpotObject FindEmptyTowerSpot()
        {
            return _towerSpots.Find(inFindItem => inFindItem.IsEmptySpot());
        }
        
        private TowerSpotObject FindAttachTowerSpot(TowerGroupObject inTowerGroup)
        {
            return _towerSpots.Find(inFindItem => inFindItem.towerGroup == inTowerGroup);
        }
        
        private TowerSpotObject FindTowerSpotByTouch(Vector3 inTouchPosition)
        {
            return _towerSpots.Find(inFindItem =>
            {
                return CommonUtils.IsPositionInsideObjectBounds(inTouchPosition, inFindItem.transform);
            });
        }
        
        private TowerGroupObject FindAttachableSameTowerGroup(TowerTableDataItem inData)
        {
            return _towerGroups.Find(inFindItem => inFindItem.IsAttachableTower() && inFindItem.IsSameTower(inData));
        }
        
        private TowerGroupObject FindTowerGroupByTouch(Vector3 inTouchPosition)
        {
            return _towerGroups.Find(inFindItem =>
            {
                return CommonUtils.IsPositionInsideObjectBounds(inTouchPosition, inFindItem.transform);
            });
        }
        #endregion
        
        public void InitContent()
        {
            // 타워 그룹 초기화
            _towerGroups.Clear();
            
            // 타워 스팟 초기화
            _towerSpots.Clear();
            _towerSpots.AddRange(towerSpotsParent.GetComponentsInChildren<TowerSpotObject>());

            // 라인 렌더러 비활성화
            lineRenderer.enabled = false;
            lineRenderer.positionCount = 0;
            
            GetTableDataManager(inMgr =>
            {
                towerTableData = inMgr.towerTableData;
            });
        }

        public void UpdateContent(float inDeltaTime)
        {
            _towerGroups.ForEach(inItem => inItem.DoMissileLaunch(inDeltaTime));
        }
        
        private TowerGroupObject CreateTowerGroup()
        {
            // 리소스 폴더에서 프리팹을 로드합니다.
            GameObject towerGroupPrefab = Resources.Load<GameObject>("LuckyDefense/Prefab/TowerGroup/TowerGroupObject");

            if (towerGroupPrefab == null)
            {
                Debug.LogError("타워 그룹 프리팹을 찾을 수 없습니다!");
                return null;
            }

            // 프리팹을 기반으로 새로운 게임 오브젝트를 생성하고 TowerGroupObject 컴포넌트를 가져옵니다.
            GameObject towerGroupInstance = Instantiate(towerGroupPrefab);
            TowerGroupObject towerGroupObject = towerGroupInstance.GetComponent<TowerGroupObject>();

            // TowerGroupObject 컴포넌트가 없다면 에러 메시지를 출력하고 null을 반환합니다.
            if (towerGroupObject == null)
            {
                Debug.LogError("생성된 타워 그룹 오브젝트에 TowerGroupObject 컴포넌트가 없습니다!");
                Destroy(towerGroupInstance); // 잘못 생성된 오브젝트는 삭제합니다.
                return null;
            }
            
            towerGroupObject.onMissileLaunch = onCreateMissile;
            // towerGroupObject.onTowerDragStart = OnTowerDragStart;
            // towerGroupObject.onTowerDragEnd = OnTowerDragEnd;
            // towerGroupObject.onTowerDraging = OnTowerDraging;
            return towerGroupObject;
        }
        
        public void CreateTower(long inCreateTowerSn)
        {
            // 타워 생성 데이터 가져 오기
            var tmpTowerData = towerTableData.FindTowerData(inCreateTowerSn);
            if (tmpTowerData == null)
            {
                Debug.LogError($"{this} 타워 데이터가 null 입니다.");
                return;
            }
            
            // MEMO : 같은 타워가 있는지 확인
            var tmpTowerGroup = FindAttachableSameTowerGroup(tmpTowerData);
            TowerSpotObject tmpTowerSpot = null;
            if (tmpTowerGroup == null)
            {
                // 빈 타워 스팟을 찾음
                tmpTowerSpot = FindEmptyTowerSpot();
            }
            else
            {
                tmpTowerSpot = FindAttachTowerSpot(tmpTowerGroup);
            }

            if (tmpTowerSpot.towerGroup == null)
            {
                var towerGroup = CreateTowerGroup();
                tmpTowerSpot.AttachTowerGroup(towerGroup);
                _towerGroups.Add(towerGroup);
            }

            tmpTowerSpot.towerGroup.CreateAttachTower(tmpTowerData);
            tmpTowerSpot.towerGroup.missileDamage = battleService.GetTowerMissileDamage(tmpTowerData.sn);
        }

        public bool IsCreateRandomTower()
        {
            // MEMO : 타워 생성 재화 체크
            if (battleService.IsTowerSummonCostEnough() == false)
            {
                Debug.LogError($"{this} 타워 생성 재화가 부족 합니다.");
                return false;
            }

            return true;
        }
        public void CreateRandomTower()
        {
            if (IsCreateRandomTower() == false)
                return;

            battleService.PayTowerSummonCost();
            
            var createSn = battleService.GetTowerSummon();
            if (createSn <= 0)
            {
                DebugLogHelper.LogError("createSn is 0", this, "CreateRandomTower");
                return;
            }
            CreateTower(createSn);
        }
        
        TowerGroupObject selectTowerGroup = null;
        TowerSpotObject selectTowerSpot = null;

        public void OnMouseDown()
        {
            var findItem = FindTowerGroupByTouch(currTouchPosition);
        
            if (findItem != null)
            {
                selectTowerGroup = findItem;
                selectTowerSpot = FindAttachTowerSpot(selectTowerGroup);
                Debug.Log(selectTowerGroup.name + " 선택됨");
                
                // 타워 그룹이 선택되면 라인 렌더러를 활성화하고 시작 위치를 설정
                lineRenderer.enabled = true;
                lineRenderer.positionCount = 2;
                lineRenderer.SetPosition(0, selectTowerGroup.transform.position);
                
                // MEMO : 타워 선택 메뉴를 오픈한다
                onOpenTowerSelectMenu?.Invoke(selectTowerGroup);
            }
            else
            {
                selectTowerGroup = null;
                onCloseTowerSelectMenu?.Invoke();
                Debug.Log("타워 그룹이 선택되지 않음");
            }
        }
        
        public void OnMouseDrag()
        {
            if (selectTowerGroup != null)
            {
                var touchTowerSpot = FindTowerSpotByTouch(currTouchPosition);

                if (touchTowerSpot == null)
                {
                    // MEMO : 터치 취소
                    selectTowerGroup = null;
                    selectTowerSpot = null;
                    
                    // 라인 그리기 해제
                    lineRenderer.enabled = false;
                    lineRenderer.positionCount = 0;
                    
                    _towerSpots.ForEach(inForItem =>
                    {
                        inForItem.view.ShowWhiteBack();
                    });
                    
                    // 메뉴 해제
                    onCloseTowerSelectMenu?.Invoke();
                    Debug.Log("터치 해제됨");
                    return;
                }
        
                if (touchTowerSpot != selectTowerSpot)
                {
                    _towerSpots.ForEach(inForItem =>
                    {
                        if(inForItem != touchTowerSpot)
                            inForItem.view.ShowWhiteBack();
                        else
                            inForItem.view.ShowRedBack();
                    });
        
                    lineRenderer.SetPosition(1, touchTowerSpot.transform.position);
                    onCloseTowerSelectMenu?.Invoke();
                }
            }
        }
        
        public void OnMouseUp()
        {
            // 타워 그룹이 이동이 가능한 위치인치 판단하여 타워 그룹을 이동 시킨다
            var findItem = FindTowerSpotByTouch(currTouchPosition);
        
            if (findItem != null && selectTowerGroup != null)
            {
                // 이전에 선택 된 타워 그룹이 있던 spot에서 타워 그룹을 제거 한다.
                selectTowerSpot.DetachTowerGroup();
                // 새로운 타워 스팟에 타워 그룹을 붙인다
                findItem.AttachTowerGroup(selectTowerGroup, false);
                
                // 타워 이동
                selectTowerGroup.AttachMove();
                
                // 선택한 타워 그룹 해제
                selectTowerGroup = null;
                selectTowerSpot = null;
                
                // 라인 그리기 해제
                lineRenderer.enabled = false;
                lineRenderer.positionCount = 0;
                Debug.Log(findItem.name + " 선택됨");
            }
        }
        
        // MEMO : 터치 관련으로 해서 하나의 유틸 함수로 뺄 고려를 해보자
        public Vector3 prevTowerPosition = Vector3.zero;
        private ITowerSelectMenu towerSelectMenuImplementation;

        public Vector3 currTouchPosition
        {
            get
            {
                Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                worldMousePosition.z = 0; // 2D 환경에서는 z축을 0으로 설정
                return worldMousePosition;
            }
        }

        #region ITowerSelectMenu

        
        public bool IsTowerUpgradeAvailable(TowerGroupObject inTowerGroup)
        {
            if (inTowerGroup.GetAttachTowerCount() < GameConfigData.maxTowerMergeCount)
            {
                Debug.Log("합쳐진 타워 갯수가 부족함");
                return false;
            }

            if (towerTableData.IsTowerUpgradeAvailable(inTowerGroup.TowerTableData) == false)
            {
                Debug.Log("더이상 티어업을 할 수 없음");
                return false;
            }
            
            if (battleService.IsTowerUpgradeCostEnough(inTowerGroup.TowerTableData.sn) == false)
            {
                Debug.Log("업그레이드 재화가 부족함");
                return false;
            }
                
            return true;
        }
        public void TowerUpgrade(TowerGroupObject inTowerGroup)
        {
            // MEMO : 한번더 체크
            if(IsTowerUpgradeAvailable(inTowerGroup) == false)
            {
                Debug.LogError($"타워 업그레이드 불가");
                return;
            }
            
            // MEMO : 타워를 업그레이드 하여 변경 한다
            // 타워 생성 데이터 가져 오기
            var randomPickSn = towerTableData.GetRandomPick(inTowerGroup.TowerTableData.grade + 1);
            CreateTower(randomPickSn);

                        
            battleService.PayTowerTierUpCost(inTowerGroup.TowerTableData.sn);
            
            var findSpot = FindAttachTowerSpot(inTowerGroup);
            findSpot.DetachTowerGroup();
            
            inTowerGroup.ClearTower();
        }

        public void TowerRemove(TowerGroupObject inTowerGroup)
        {
            // MEMO : 타워를 제거한다
            battleService.RefundTowerCost(inTowerGroup.TowerTableData.sn);
            
            var findSpot = FindAttachTowerSpot(inTowerGroup);
            findSpot.DetachTowerGroup();
            
            inTowerGroup.ClearTower();
        }

        #endregion ITowerSelectMenu
        
        
    }
}