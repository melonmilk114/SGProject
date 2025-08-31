using System;
using System.Collections.Generic;
using UnityEngine;

namespace Melon
{
    public static class CommonUtils
    {
        // MEMO : Get함수는 보통 데이터가 반드시 존재한다고 가정하고 동작
        // MEMO : Find함수는 데이터가 없을 수도 있으므로 예외를 던지지 않고 null이나 default 값을 반환하는 경우가 많음
        #region FindObject
        
        public static List<T> FindChildObjects<T>(GameObject inParent, bool inIsCheckActiveInHierarchy = false) where T : class
        {
            List<T> resultList = new List<T>();
            FindChildObjectsRecursive(inParent.transform, resultList, inIsCheckActiveInHierarchy);
            return resultList;
        }

        private static void FindChildObjectsRecursive<T>(Transform inParent, List<T> inResultList, bool inIsCheckActiveInHierarchy = false) where T : class
        {
            foreach (Transform child in inParent)
            {
                // 모든 컴포넌트를 가져와서 인터페이스를 구현한 컴포넌트만 추가
                foreach (var component in child.GetComponents<Component>())
                {
                    if (inIsCheckActiveInHierarchy && component.gameObject.activeInHierarchy == false)
                        continue;
                    
                    if (component is T target)
                    {
                        inResultList.Add(target);
                    }
                }

                // 재귀 호출로 자식의 자식까지 탐색
                FindChildObjectsRecursive(child, inResultList);
            }
        }
        
        #endregion
        
        #region FindComponent
        public static T FindComponent<T>(GameObject inObj, bool inIsCheckActiveInHierarchy = false) where T : class
        {
            if (inObj == null) return null;
            
            foreach (var component in inObj.GetComponents<Component>())
            {
                if (inIsCheckActiveInHierarchy && component.gameObject.activeInHierarchy == false)
                    continue;

                if (component is T target)
                {
                    return target;
                }
            }

            return null;
        }
        public static List<T> FindComponents<T>(GameObject inObj, bool inIsCheckActiveInHierarchy = false) where T : class
        {
            var resultList = new List<T>();
            if (inObj == null) return resultList;
            
            foreach (var component in inObj.GetComponents<Component>())
            {
                if (inIsCheckActiveInHierarchy && component.gameObject.activeInHierarchy == false)
                    continue;

                if (component is T target)
                {
                    resultList.Add(target);
                }
            }

            return resultList;
        }
        #endregion FindComponent

        #region FileLoad

        public static TextAsset LoadTextAsset(string inFilePath)
        {
            TextAsset textAsset = Resources.Load<TextAsset>(inFilePath);
            if (textAsset == null)
            {
                Debug.LogError($"LoadTextAsset Error : {inFilePath}");
            }
            return textAsset;
        }

        #endregion

        #region TouchInside

        public static bool IsPositionInsideObjectBounds(Vector3 worldPosition, Transform targetTransform)
        {
            Vector3 objPos = targetTransform.transform.position;
            Vector3 scale = targetTransform.transform.lossyScale;

            // 기본 크기: 임의로 1x1 유닛이라고 가정
            float width = 1f * scale.x;
            float height = 1f * scale.y;

            // AABB 영역 계산
            bool isInside =
                worldPosition.x >= objPos.x - width / 2f &&
                worldPosition.x <= objPos.x + width / 2f &&
                worldPosition.y >= objPos.y - height / 2f &&
                worldPosition.y <= objPos.y + height / 2f;
            
            //Debug.LogError($"IsPositionInsideObjectBounds {targetTransform.name} : Inside = {isInside}");
            return isInside;
        }

        #endregion
        
        public static T AddComponent<T>(GameObject inObj, string inTypeName) where T : Component
        {
            if (string.IsNullOrEmpty(inTypeName)) return null;

            Type type = Type.GetType(inTypeName);
            if (type == null)
            {
                Debug.LogError($"[MonsterObject] 타입을 찾을 수 없음: {inTypeName}");
                return null;
            }

            // 이미 붙어있다면 제거
            var oldHandler = inObj.GetComponent(type);
            if (oldHandler != null) UnityEngine.Object.Destroy(oldHandler);

            // 새로 붙이기
            var newHandler = inObj.AddComponent(type) as T;
            if (newHandler == null)
            {
                Debug.LogError($"[MonsterObject] {inTypeName} 컴포넌트를 {typeof(T).Name}으로 캐스팅 실패");
            }

            return newHandler;
        }
        public static void AllRemoveComponent<T>(this GameElement inObj) where T : Component
        {
            var components = inObj.GetComponents<T>();
            foreach (var component in components)
            {
                UnityEngine.Object.Destroy(component);
            }
        }
        
        public static T EnumParse<T>(this string value, T defaultValue = default) where T : struct, Enum
        {
            if (string.IsNullOrEmpty(value))
                return defaultValue;

            if (Enum.TryParse<T>(value, true, out var result))
                return result;

            return defaultValue;
        }
    }
}