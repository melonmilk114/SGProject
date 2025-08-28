using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngineInternal;

namespace Melon
{
    public class GameElement : MonoBehaviour, IGameElement
    {
        public bool isInit = false;
        
        public void Awake()
        {
            OnAwakeFunc();
        }

        /// <summary>
        /// Awake시 호출됨
        /// </summary>
        public virtual void OnAwakeFunc()
        {
        }
        
        public void Start()
        {
            OnStartFunc();
        }

        public virtual void OnStartFunc()
        {
        }
        public void OnEnable()
        {
            OnEnableFunc();
        }
    
        public void OnDisable()
        {
            OnDisableFunc();
        }
    
        public void OnDestroy()
        {
            OnDestroyFunc();
        }
    
        public virtual void OnEnableFunc()
        {
            
        }
        
        public virtual void OnDisableFunc()
        {
            
        }
        
        public virtual void OnDestroyFunc()
        {
            
        }

        public virtual void DoPreShow(object inData = null)
        {
            
        }
        public virtual void DoPreShow(object inData = null, ActionResult inActionResult = null)
        {
            DoPreShow(inData);
            inActionResult?.OnSuccess();
        }
        public virtual void DoPostShow(object inData = null)
        {
            
        }
        public virtual void DoPostShow(object inData = null, ActionResult inActionResult = null)
        {
            DoPostShow(inData);
            inActionResult?.OnSuccess();
        }
        public virtual void DoShowCheck(object inData = null)
        {
            
        }
        public virtual void DoShowCheck(object inData = null, ActionResult inActionResult = null)
        {
            DoShowCheck(inData);
            inActionResult?.OnSuccess();
        }
        public void DoShow(object inData = null, ActionResult inActionResult = null)
        {
            if (inActionResult == null)
            {
                DoShowCheck(inData);
                DoPreShow(inData);
                
                gameObject.SetActive(true);
                
                DoPostShow(inData);
            }
            else
            {
                DoShowCheck(inData, new ActionResult()
                {
                    onSuccess = () =>
                    {
                        DoPreShow(inData, new ActionResult()
                        {
                            onSuccess = () =>
                            {
                                gameObject.SetActive(true);
                            
                                DoPostShow(inData, new ActionResult()
                                {
                                    onSuccess = () =>
                                    {
                                        inActionResult?.OnSuccess();
                                    },
                                    onFail = (err) =>
                                    {
                                        Debug.LogError(err);
                                        inActionResult?.OnFail(err);
                                    }
                                });
                            },
                            onFail = (err) =>
                            {
                                Debug.LogError(err);
                                inActionResult?.OnFail(err);
                            }
                        });
                    },
                    onFail = (err) =>
                    {
                        Debug.LogError(err);
                        inActionResult?.OnFail(err);
                    }
                });
            }
        }
        public virtual void DoPreHide(object inData = null)
        {
            
        }
        public virtual void DoPreHide(object inData = null, ActionResult inActionResult = null)
        {
            DoPreHide(inData);
            inActionResult?.OnSuccess();
        }
        public virtual void DoPostHide(object inData = null)
        {
            
        }
        public virtual void DoPostHide(object inData = null, ActionResult inActionResult = null)
        {
            DoPostHide(inData);
            inActionResult?.OnSuccess();
        }
        public virtual void DoHideCheck(object inData = null)
        {
            
        }
        public virtual void DoHideCheck(object inData = null, ActionResult inActionResult = null)
        {
            DoHideCheck(inData);
            inActionResult?.OnSuccess();
        }
    
        public virtual void DoHide(object inData = null, ActionResult inActionResult = null)
        {
            if (inActionResult == null)
            {
                DoHideCheck(inData);
                DoPreHide(inData);
                
                gameObject.SetActive(false);
                
                DoPostHide(inData);
            }
            else
            {
                DoHideCheck(inData, new ActionResult()
                {
                    onSuccess = () =>
                    {
                        DoPreHide(inData, new ActionResult()
                        {
                            onSuccess = () =>
                            {
                                gameObject.SetActive(false);
                            
                                DoPostHide(inData, new ActionResult()
                                {
                                    onSuccess = () =>
                                    {
                                        inActionResult?.OnSuccess();
                                    },
                                    onFail = (err) =>
                                    {
                                        Debug.LogError(err);
                                        inActionResult?.OnFail(err);
                                    }
                                });
                            },
                            onFail = (err) =>
                            {
                                Debug.LogError(err);
                                inActionResult?.OnFail(err);
                            }
                        });
                    },
                    onFail = (err) =>
                    {
                        Debug.LogError(err);
                        inActionResult?.OnFail(err);
                    }
                });
            }
        }

        public virtual void Init()
        {
            isInit = true;
        }
    }
}

