using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Melon
{
    public class ObjectPoolManager : MonoBehaviour , IFrameworkModule
    {
        #region Singleton
        private static ObjectPoolManager _instance = null;
        public static ObjectPoolManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<ObjectPoolManager>();
                }
                return _instance;
            }
        }
        #endregion
        
        public GameObject poolRootObj;
        public Dictionary<string, Queue<GameElement>> objectPools = new Dictionary<string, Queue<GameElement>>();
        private Dictionary<string, HashSet<GameElement>> objectSeen = new Dictionary<string, HashSet<GameElement>>();
        
        // 오브젝트풀에 등록 방법 (모두 사용 가능)
        // 1. 프리팹을 직접 등록 (키 : IObjectPoolUnit를 상속 받은 클래스 명)
        // 2. 런타임으로 프리팹 경로를 통해 등록 가능 (키 : IObjectPoolUnit를 상속 받은 클래스 명)
        // 3. IObjectPoolUnit를 상속 받은 클래스가 Awake때 스스로를 등록 가능 (키 : 클래스명)
        
        public List<GameElement> prefabList = new List<GameElement>();
        
        public Dictionary<string, GameElement> templatePools = new Dictionary<string, GameElement>();
        
        public void InitModule()
        {
            prefabList.ForEach(RegisterToPoolByPrefab);
            
            
            var configurators = FindObjectsOfType<MonoBehaviour>(true)
                .OfType<IObjectPoolConfigurator>();

            foreach (var config in configurators)
            {
                foreach (var prefab in config.GetPoolPrefabs())
                {
                    if (prefab != null)
                        RegisterToPoolByPrefab(prefab);
                }
            }

            Debug.Log($"[ObjectPoolManager] 풀 구성 요소 {configurators.Count()}개에서 프리팹 등록 완료");
        }
        
        public void RegisterToPool(IObjectPoolUnit inPoolUnit)
        {
            // 풀에 넣을 오브젝트 여러개를 생성 해서 넣음
            string unitName = inPoolUnit.GetType().FullName;
            if (templatePools.ContainsKey(unitName))
            {
                Debug.LogError($"RegisterToPool Error(overlapKey) : {unitName}");
                return;
            }
            
            if(inPoolUnit is GameElement castObj)
            {
                var newTmpObj = Instantiate(castObj, poolRootObj.transform);
                EnqueueTemplatePool(newTmpObj);
                
                for (int repeatCount = 0; repeatCount < 30; repeatCount++)
                {
                    var newObj = Instantiate(castObj, poolRootObj.transform);
                    EnqueuePool(newObj);
                }
            }
            else
            {
                Debug.LogError("RegisterToPool Error");
            }
        }
        
        public void RegisterToPool(GameElement inPoolUnit)
        {
            if (inPoolUnit is IObjectPoolUnit castObj)
            {
                RegisterToPool(castObj);
            }
        }

        public void RegisterToPoolByPrefab(GameElement inPrefab)
        {
            RegisterToPool(inPrefab);
        }
        
        public void RegisterToPoolByPrefabPath(string inPrefabPath)
        {
            var tmpPrefab = Resources.Load<GameElement>(inPrefabPath);
            
            if (tmpPrefab != null)
                RegisterToPoolByPrefab(tmpPrefab);
            else
            {
                Debug.LogError($"RegisterToPoolByPrefabPath Error{inPrefabPath}");
            }
        }
        
        public void RegisterToPoolBySelf(IObjectPoolUnit inPoolUnit)
        {
            RegisterToPool(inPoolUnit);
        }
        
        public void EnqueuePool(GameElement inObj)
        {
            if (inObj is IObjectPoolUnit castObj)
            {
                string unitName = castObj.GetType().FullName;

                if (objectPools.ContainsKey(unitName) == false)
                {
                    objectPools[unitName] = new Queue<GameElement>();
                    objectSeen[unitName] = new HashSet<GameElement>();
                }

                if (objectSeen[unitName].Contains(inObj) == false)
                {
                    inObj.transform.SetParent(poolRootObj.transform, true);
                    inObj.transform.localScale = Vector3.one;
                    objectPools[unitName].Enqueue(inObj);
                    objectSeen[unitName].Add(inObj);
                }
            }   
        }

        public void EnqueueTemplatePool(GameElement inObj)
        {
            if (inObj is IObjectPoolUnit castObj)
            {
                string unitName = castObj.GetType().FullName;

                if (templatePools.ContainsKey(unitName) == false)
                    templatePools.Add(unitName, inObj);
            }
        }

    

        public T DequeuePool<T>(GameObject inParentObj) where T : GameElement
        {
            string unitName = typeof(T).FullName;
            if (objectPools.ContainsKey(unitName))
            {
                Func<T> getObject = () =>
                {
                    if (objectPools[unitName].TryDequeue(out var peekObj))
                    {
                        objectSeen[unitName].Remove(peekObj);
                        
                        if(peekObj is T returnObj)
                            return returnObj;
                    }

                    return null;
                };

                var returnObj = getObject.Invoke();
                if (returnObj == null)
                {
                    if(templatePools.ContainsKey(unitName) == false)
                    {
                        Debug.LogError($"DequeuePool Error(Null) : {unitName}");
                        return null;
                    }
                    else
                    {
                        var newObj = Instantiate(templatePools[unitName], poolRootObj.transform);
                        EnqueuePool(newObj);
                    }
                    
                    returnObj = getObject.Invoke();
                    returnObj.transform.SetParent(inParentObj.transform, false);
                    return returnObj;
                }

                returnObj.transform.SetParent(inParentObj.transform, false);
                return returnObj;
            }
            Debug.LogError("RegisterToPool Error");
            return null;
        }

        
    }
}