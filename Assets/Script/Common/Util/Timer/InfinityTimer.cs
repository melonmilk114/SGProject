using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Melon
{
    public class InfinityTimer : MonoBehaviour
    {
        public class Data
        {
            public float startTime = 0;
            public float currentTime = 0;

            public float tickUnitTime = 0;
            
            public bool isUnscaledTime = false;

            public bool isRemove = false;
            public bool isFinish = false;

            public Action onTickAction = null;

            public void ResetData()
            {
                startTime = 0;
                currentTime = 0;
                tickUnitTime = 0;
                isUnscaledTime = false;
                isRemove = false;
                isFinish = false;
                onTickAction = null;
            }
        }

        public InfinityTimer.Data data = new InfinityTimer.Data();
        protected Coroutine delayCoroutine = null;
        
        private void OnDisable()
        {
            if (Application.isPlaying == false) return;
            
            DoRemove();
        }
        
        public virtual void Init(float inTickUnitTime, Action inTickAction, bool inIsUnscaledTime = false)
        {
            data.ResetData();
            
            data.isUnscaledTime = inIsUnscaledTime;
            
            data.tickUnitTime = inTickUnitTime;
            
            data.onTickAction = inTickAction;
        }

        public InfinityTimer DoStart(float inTickUnitTime, Action inTickAction, bool inIsUnscaledTime = false)
        {
            Init(inTickUnitTime, inTickAction, inIsUnscaledTime);

            if (gameObject.activeInHierarchy == false)
            {
                Debug.Log("(W> " + this +".gameObject.activeInHierarchy == false");
                DoRemove();
            }
            else
            {
                InfinityTimerMgr.Instance?.AddOfInfinityTimer(this);

                StopAllCoroutines();
                delayCoroutine = StartCoroutine(DoStartForCoroutine());
            }

            return this;
        }
        
        public void DoTick()
        {
            data.onTickAction?.Invoke();
        }

        protected virtual IEnumerator DoStartForCoroutine()
        {
            data.startTime = data.isUnscaledTime ? Time.realtimeSinceStartup : Time.time;
            data.startTime = Mathf.FloorToInt(data.startTime);
            data.currentTime = data.startTime;
            float nextTickActionInvokeTime = data.startTime + data.tickUnitTime;
            bool isInvokeFinish = false;
            while (isInvokeFinish == false && data.isRemove == false)
            {
                yield return new WaitForEndOfFrame();

                if (!data.isFinish)
                {
                    if (data.isUnscaledTime)
                        data.currentTime += Time.unscaledDeltaTime;
                    else
                        data.currentTime += Time.smoothDeltaTime;
                    
                    if (nextTickActionInvokeTime <= data.currentTime)
                    {
                        nextTickActionInvokeTime = data.currentTime + data.tickUnitTime;
                        DoTick();
                    }
                }
            }
        }

        public void DoRemove()
        {
            // 딜레이 제거
            if (data == null) return;
            else if (data.isRemove) return;
            
            data.isRemove = true;
            data.isFinish = true;

            if (delayCoroutine != null)
                StopCoroutine(delayCoroutine);
            delayCoroutine = null;
            
            Destroy(this);
        }
        
        public static InfinityTimer CreateInfinityTimer(float inTickUnitTime, Action inTickAction, bool inIsUnscaledTime = false)
        {
            // Debug.Log("(SF> TimerObj.Delay() : " + inOwner +", "+ inTime +", "+inOnFinish +", "+ isUnscaledTime);

            InfinityTimer lReturn = null;
            try
            {
                var lGameObject = InfinityTimerMgr.Instance?.gameObject;
                lReturn = lGameObject.AddComponent<InfinityTimer>();
                lReturn.DoStart(inTickUnitTime, inTickAction, inIsUnscaledTime);
            }
            catch (Exception e)
            {
                Debug.LogError("CreateInfinityTimer " + e);
            }

            return lReturn;
        }
    }
    
    #region Manager

    public class InfinityTimerMgr : MonoBehaviour
    {
        #region Singleton

        protected static InfinityTimerMgr _instance = null;
        public static InfinityTimerMgr Instance
        {
            get
            {
                if (IsApplicationQuit) return null;
                else if (_instance == null)
                {
                    _instance = GameObject.FindObjectOfType(typeof(InfinityTimerMgr)) as InfinityTimerMgr;
                    if (_instance == null)
                    {
                        GameObject go = new GameObject(typeof(InfinityTimerMgr).ToString(), typeof(InfinityTimerMgr));
                        _instance = go.GetComponent<InfinityTimerMgr>();
                        //mInstance.Init();
                    }
                }

                return _instance;
            }
        }

        protected static bool _isApplicationQuit = false;
        public static bool IsApplicationQuit
        {
            get { return _isApplicationQuit;  }
        }

        protected virtual void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
                return;
            }

            if (_instance == null)
            {
                _isApplicationQuit = false;
                _instance = this;
            }
        }

        protected virtual void OnDestroy()
        {
            _instance = null;
        }

        protected virtual void OnApplicationQuit()
        {
            _isApplicationQuit = true;
            _instance = null;
        }

        #endregion Singleton

        #region Info

        public List<InfinityTimer> timers = new List<InfinityTimer>();

        #endregion Info
        
        #region Add

        public void AddOfInfinityTimer(InfinityTimer inObj)
        {
            if (inObj == null) return;

            if (!timers.Contains(inObj))
            {
                timers.Add(inObj);
            }
        }
        
        #endregion Add
    }

    #endregion Manager
}