using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Melon
{
    public class CountDownTimer : MonoBehaviour
    {
        public class Data
        {
            public float startTime = 0;
            public float finishTime = 0;
            public float currentTime = 0;

            public float tickUnitTime = 0;
            
            public bool isUnscaledTime = false;
            public float percent = 0;

            public bool isRemove = false;
            public bool isFinish = false;

            public Action onTickAction = null;
            public Action onFinishAction = null;

            public void ResetData()
            {
                startTime = 0;
                finishTime = 0;
                currentTime = 0;
                tickUnitTime = 0;
                isUnscaledTime = false;
                percent = 0;
                isRemove = false;
                isFinish = false;
                onTickAction = null;
                onFinishAction = null;
            }
        }

        public CountDownTimer.Data data = new CountDownTimer.Data();
        protected Coroutine delayCoroutine = null;
        
        private void OnDisable()
        {
            if (Application.isPlaying == false) return;
            
            DoRemove();
        }
        
        public virtual void Init(float inFinishTime, System.Action inFinishAction, float inTickUnitTime, Action inTickAction, bool inIsUnscaledTime = false)
        {
            data.ResetData();
            
            data.isUnscaledTime = inIsUnscaledTime;
            
            data.finishTime = this.data.isUnscaledTime ? Time.realtimeSinceStartup : Time.time;
            data.finishTime += inFinishTime;
            data.tickUnitTime = inTickUnitTime;
            
            data.onTickAction = inTickAction;
            data.onFinishAction = inFinishAction;
        }

        public CountDownTimer DoStart(float inFinishTime, System.Action inOnFinish, float inTickUnitTime, Action inTickAction, bool inIsUnscaledTime = false)
        {
            Init(inFinishTime, inOnFinish, inTickUnitTime, inTickAction, inIsUnscaledTime);

            if (gameObject.activeInHierarchy == false)
            {
                Debug.Log("(W> " + this +".gameObject.activeInHierarchy == false");
                DoRemove();
            }
            else
            {
                CountDownTimerMgr.Instance?.AddOfCountDownTimer(this);

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
        
        public void DoTick()
        {
            // 딜레이 종료
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

                    data.percent = (data.currentTime - data.startTime) / (data.finishTime - data.startTime);
                    
                    isInvokeFinish = data.currentTime >= data.finishTime;

                    if (nextTickActionInvokeTime <= data.currentTime)
                    {
                        nextTickActionInvokeTime = data.currentTime + data.tickUnitTime;
                        DoTick();
                    }
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
        
        public static CountDownTimer CreateCountDownTimer(float inFinishTime, System.Action inOnFinish, float inTickUnitTime, Action inTickAction, bool inIsUnscaledTime = false)
        {
            // Debug.Log("(SF> TimerObj.Delay() : " + inOwner +", "+ inTime +", "+inOnFinish +", "+ isUnscaledTime);

            CountDownTimer lReturn = null;
            try
            {
                var lGameObject = CountDownTimerMgr.Instance?.gameObject;
                lReturn = lGameObject.AddComponent<CountDownTimer>();
                lReturn.DoStart(inFinishTime, inOnFinish, inTickUnitTime, inTickAction, inIsUnscaledTime);
            }
            catch (Exception e)
            {
                Debug.LogError("CreateCountDownTimer " + e);
            }

            return lReturn;
        }
    }
    
    #region Manager

    public class CountDownTimerMgr : MonoBehaviour
    {
        #region Singleton

        protected static CountDownTimerMgr _instance = null;
        public static CountDownTimerMgr Instance
        {
            get
            {
                if (IsApplicationQuit) return null;
                else if (_instance == null)
                {
                    _instance = GameObject.FindObjectOfType(typeof(CountDownTimerMgr)) as CountDownTimerMgr;
                    if (_instance == null)
                    {
                        GameObject go = new GameObject(typeof(CountDownTimerMgr).ToString(), typeof(CountDownTimerMgr));
                        _instance = go.GetComponent<CountDownTimerMgr>();
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

        public List<CountDownTimer> timers = new List<CountDownTimer>();

        #endregion Info
        
        #region Add

        public void AddOfCountDownTimer(CountDownTimer inObj)
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