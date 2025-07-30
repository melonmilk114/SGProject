using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Melon
{
    /// <summary>
    /// 특정 시간 이후에 함수 실행
    /// </summary>
    public class DelayTimer : MonoBehaviour
    {
        public class Data
        {
            public float startTime = 0;
            public float finishTime = 0;
            public float currentTime = 0;
            
            public bool isUnscaledTime = false;
            public float percent = 0;

            public bool isRemove = false;
            public bool isFinish = false;

            public Action onFinishAction = null;

            public void ResetData()
            {
                finishTime = 0;
                currentTime = 0;
                isUnscaledTime = false;
                percent = 0;
                isRemove = false;
                isFinish = false;
                onFinishAction = null;
            }
        }

        public Data data = new Data();
        protected Coroutine delayCoroutine = null;
        
        private void OnDisable()
        {
            if (Application.isPlaying == false) return;
            
            DoRemove();
        }
        
        public virtual void Init(float inDelayTime, System.Action inFinishAction, bool inIsUnscaledTime = false)
        {
            data.ResetData();
            
            data.isUnscaledTime = inIsUnscaledTime;
            
            data.finishTime = this.data.isUnscaledTime ? Time.realtimeSinceStartup : Time.time;
            data.finishTime += inDelayTime;

            data.onFinishAction = inFinishAction;
        }

        public DelayTimer DoStart(float inTime, System.Action inOnFinish, bool inIsUnscaledTime = false)
        {
            Init(inTime, inOnFinish, inIsUnscaledTime);

            if (gameObject.activeInHierarchy == false)
            {
                Debug.Log("(W> " + this +".gameObject.activeInHierarchy == false");
                DoRemove();
            }
            else
            {
                DelayTimerMgr.Instance?.AddOfDelayTimer(this);

                StopAllCoroutines();
                delayCoroutine = StartCoroutine(DoStartForCoroutine());
            }

            return this;
        }

        public void DoFinish()
        {
            // 딜레이 종료
            data.onFinishAction?.Invoke();
            DoRemove();
        }

        protected virtual IEnumerator DoStartForCoroutine()
        {
            data.startTime = data.isUnscaledTime ? Time.realtimeSinceStartup : Time.time;
            data.currentTime = data.startTime;
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

                    data.percent = (data.currentTime - data.startTime) / (data.finishTime - data.startTime);
                    
                    isInvokeFinish = data.currentTime >= data.finishTime;
                }
            }

            DoFinish();
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
        
        public static DelayTimer CreateDelayTimer(float inTime, System.Action inOnFinish, bool inIsUnscaledTime = false)
        {
            // Debug.Log("(SF> TimerObj.Delay() : " + inOwner +", "+ inTime +", "+inOnFinish +", "+ isUnscaledTime);

            DelayTimer lReturn = null;
            try
            {
                var lGameObject = DelayTimerMgr.Instance?.gameObject;
                lReturn = lGameObject.AddComponent<DelayTimer>();
                lReturn.DoStart(inTime, inOnFinish, inIsUnscaledTime);
            }
            catch (Exception e)
            {
                Debug.LogError("CreateDelayTimer " + e);
            }

            return lReturn;
        }
        
        public static DelayTimer CreateDelayNextFrameTimer(System.Action inOnFinish, bool inIsUnscaledTime = false)
        {
            // Debug.Log("(SF> TimerObj.Delay() : " + inOwner +", "+ inTime +", "+inOnFinish +", "+ isUnscaledTime);

            DelayTimer lReturn = null;
            try
            {
                var lGameObject = DelayTimerMgr.Instance?.gameObject;
                lReturn = lGameObject.AddComponent<DelayTimer>();
                lReturn.DoStart(0f, inOnFinish, inIsUnscaledTime);
            }
            catch (Exception e)
            {
                Debug.LogError("CreateDelayTimer " + e);
            }

            return lReturn;
        }
    }
    
    #region Manager

    public class DelayTimerMgr : MonoBehaviour
    {
        #region Singleton

        protected static DelayTimerMgr _instance = null;
        public static DelayTimerMgr Instance
        {
            get
            {
                if (IsApplicationQuit) return null;
                else if (_instance == null)
                {
                    _instance = GameObject.FindObjectOfType(typeof(DelayTimerMgr)) as DelayTimerMgr;
                    if (_instance == null)
                    {
                        GameObject go = new GameObject(typeof(DelayTimerMgr).ToString(), typeof(DelayTimerMgr));
                        _instance = go.GetComponent<DelayTimerMgr>();
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

        public List<DelayTimer> timers = new List<DelayTimer>();

        #endregion Info
        
        #region Add

        public void AddOfDelayTimer(DelayTimer inObj)
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