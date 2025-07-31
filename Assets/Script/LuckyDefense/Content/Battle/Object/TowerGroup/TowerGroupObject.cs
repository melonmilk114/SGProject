using System;
using System.Collections;
using System.Collections.Generic;
using Melon;
using UnityEngine;

namespace LuckyDefense
{
    public class TowerGroupObject : GameElement
    {
        public TowerGroupView view;
        
        public List<TowerObject> towerList = new List<TowerObject>();
        public TowerTableDataItem TowerTableData
        {
            get
            {
                if (towerList.Count == 0)
                    return null;

                return towerList[0].tableData;
            }
        }

        public long missileDamage = 0;
        
        public Action<TowerGroupObject> onMissileLaunch = null;
        
        public Coroutine moveCoroutine = null;
        public bool isMoving = false;
        
        public bool IsEmptySpot()
        {
            return towerList.Count == 0;
        }
        
        public bool IsSameTower(TowerTableDataItem inData)
        {
            return inData.sn == TowerTableData?.sn;
        }
        
        public bool IsAttachableTower()
        {
            if (towerList.Count >= GameConfigData.maxTowerMergeCount)
                return false;

            return true;
        }

        public bool IsInAttackRange(Transform inTarget)
        {
            float distance = Vector3.Distance(transform.position, inTarget.position);
            return distance <= TowerTableData?.attack_range / 2;
        }

        public int GetAttachTowerCount()
        {
            return towerList.Count;
        }

        public void CreateAttachTower(TowerTableDataItem inData)
        {
            GameElement prefab = Resources.Load<GameElement>(inData?.prefabPath);

            // 프리팹이 제대로 로드되었는지 확인합니다.
            if (prefab != null)
            {
                // 프리팹을 현재 위치에 생성합니다.
                var newPrefab = Instantiate(prefab.gameObject, transform);
                var comp = newPrefab.GetComponent<TowerObject>();
                comp.SetData(inData);
                AttachTower(comp);
            }
            else
            {
                Debug.LogError($"{this} 타워 프리팹을 찾을 수 없습니다.");
            }
        }

        public void AttachTower(TowerObject inTower)
        {
            inTower.OnAttachTower((inMissileSn) =>
            {
                onMissileLaunch?.Invoke(this);
            });
            inTower.transform.SetParent(transform);
            
            towerList.Add(inTower);

            if (towerList.Count == 1)
            {
                towerList[0].transform.localPosition = Vector3.zero;
                towerList[0].transform.localScale = Vector3.one;
            }
            else if (towerList.Count == 2)
            {
                towerList[0].transform.localPosition = new Vector3(-0.25f, 0.25f);
                towerList[0].transform.localScale = new Vector3(0.5f, 0.5f);
                
                towerList[1].transform.localPosition = new Vector3(0.25f, -0.25f);
                towerList[1].transform.localScale = new Vector3(0.5f, 0.5f);
            }
            else if (towerList.Count == 3)
            {
                towerList[0].transform.localPosition = new Vector3(-0.25f, 0.25f);
                towerList[0].transform.localScale = new Vector3(0.5f, 0.5f);
                
                towerList[1].transform.localPosition = new Vector3(0.25f, -0.25f);
                towerList[1].transform.localScale = new Vector3(0.5f, 0.5f);
                
                towerList[2].transform.localPosition = new Vector3(-0.25f, -0.25f);
                towerList[2].transform.localScale = new Vector3(0.5f, 0.5f);
            }
            else
            {
                Debug.LogError("타워는 최대 3개까지 장착할 수 있습니다.");
            }
        }

        public void ClearTower()
        {
            if (towerList.Count == 0) return;
    
            // 타워들을 안전하게 제거
            for (int i = towerList.Count - 1; i >= 0; i--)
            {
                RemoveTower(towerList[i]);
            }
    
            towerList.Clear();
        }

        public void RemoveTower(TowerObject inTower)
        {
            if (inTower == null) return;
    
            try
            {
                // 콜백 해제
                inTower.OnAttachTower(null);
        
                // 부모 관계 해제
                inTower.transform.SetParent(null);
        
                Destroy(inTower.gameObject);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"{this} 타워 제거 중 오류 발생: {e.Message}");
            }
        }
        
        public void AttachMove()
        {
            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
                moveCoroutine = null;
            }
            moveCoroutine = StartCoroutine(CoAttachMove());
        }

        private IEnumerator CoAttachMove()
        {
            isMoving = true;
            while (Vector3.Distance(transform.localPosition, Vector3.zero) > 0.01f) // 목표 지점에 충분히 가까워질 때까지 반복
            {
                // 현재 위치에서 목표 위치로 이동하는 방향 벡터 계산
                Vector3 direction = (Vector3.zero - transform.localPosition).normalized;

                // 프레임마다 이동
                transform.localPosition += direction * 2f * Time.deltaTime;

                yield return null; // 다음 프레임까지 대기
            }

            // 최종적으로 목표 위치에 정확히 맞춰줍니다. (오차 방지)
            transform.localPosition = Vector3.zero;
            isMoving = false;
            yield return null;
        }
        
        public void DoMissileLaunch(float inDeltaTime)
        {
            towerList.ForEach(inItem => inItem.DoMissileLaunch(inDeltaTime));
        }
    }
}