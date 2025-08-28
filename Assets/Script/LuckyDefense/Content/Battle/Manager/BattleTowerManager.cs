using System;
using System.Collections.Generic;
using Melon;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LuckyDefense
{
    public class BattleTowerManager : GameElement
    , ITargetObjectReceiver<TableDataManager>
    {
        [SerializeField] private List<TowerSpotObject> _towerSpots = new List<TowerSpotObject>();
        [SerializeField] private List<TowerGroupObject> _towerGroups = new List<TowerGroupObject>();
        
        
        
        public GameObject towerSpotsParent;
        
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

        public TowerSpotObject FindEmptyTowerSpot()
        {
            return _towerSpots.Find(inFindItem => inFindItem.IsEmptySpot());
        }
        
        public TowerSpotObject FindAttachTowerSpot(TowerGroupObject inTowerGroup)
        {
            return _towerSpots.Find(inFindItem => inFindItem.towerGroup == inTowerGroup);
        }
        
        public TowerSpotObject FindTowerSpotByTouch(Vector3 inTouchPosition)
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
        
        public TowerGroupObject FindTowerGroupByTouch(Vector3 inTouchPosition)
        {
            return _towerGroups.Find(inFindItem =>
            {
                return CommonUtils.IsPositionInsideObjectBounds(inTouchPosition, inFindItem.transform);
            });
        }
        
        public List<TowerGroupObject> FindMissileLaunchReadyTowerGroupList()
        {
            return _towerGroups.FindAll(inFindItem => inFindItem.GetMissileLaunchReadyTowerList().Count > 0);
        }
        
        #endregion
        
        public void InitContent()
        {
            // 타워 그룹 초기화
            _towerGroups.Clear();
            
            // 타워 스팟 초기화
            _towerSpots.Clear();
            _towerSpots.AddRange(towerSpotsParent.GetComponentsInChildren<TowerSpotObject>());

            GetTableDataManager(inMgr =>
            {
                towerTableData = inMgr.towerTableData;
            });
        }

        // 미사일 발사 준비가 된 타워 리스트를 갱신
        public void UpdateMissileLaunchReadyTowerList()
        {
            _towerGroups.ForEach(inItem => inItem.UpdateMissileLaunchReadyTowerList());
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
            
            // towerGroupObject.onTowerDragStart = OnTowerDragStart;
            // towerGroupObject.onTowerDragEnd = OnTowerDragEnd;
            // towerGroupObject.onTowerDraging = OnTowerDraging;
            return towerGroupObject;
        }
        
        public bool CreateTower(long inCreateTowerSn)
        {
            // 타워 생성 데이터 가져 오기
            var tmpTowerData = towerTableData.FindTowerData(inCreateTowerSn);
            if (tmpTowerData == null)
            {
                Debug.LogError($"{this} 타워 데이터가 null 입니다.");
                return false;
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

            if (tmpTowerSpot == null)
            {
                DebugLogHelper.LogError($"타워를 생성할 곳이 없습니다.");
                return false;
            }

            if (tmpTowerSpot.towerGroup == null)
            {
                var towerGroup = CreateTowerGroup();
                tmpTowerSpot.AttachTowerGroup(towerGroup);
                _towerGroups.Add(towerGroup);
            }

            tmpTowerSpot.towerGroup.CreateAttachTower(tmpTowerData);

            return true;
        }
        
        public void ChangeAllTowerSpotWhiteBack()
        {
            _towerSpots.ForEach(inItem => inItem.view.ShowWhiteBack());
        }
        public void ChangeTowerSpotWhiteBack(TowerSpotObject inTowerSpot)
        {
            _towerSpots.ForEach(inItem =>
            {
                if(inItem == inTowerSpot)
                    inItem.view.ShowWhiteBack();
            });
        }
        
        public void ChangeTowerSpotRedBack(TowerSpotObject inTowerSpot)
        {
            _towerSpots.ForEach(inItem =>
            {
                if(inItem == inTowerSpot)
                    inItem.view.ShowRedBack();
            });
        }
        
        public void TowerRemove(TowerGroupObject inTowerGroup)
        {
            var findSpot = FindAttachTowerSpot(inTowerGroup);
            findSpot.DetachTowerGroup();
            
            inTowerGroup.ClearTower();
            _towerGroups.Remove(inTowerGroup);
        }
    }
}