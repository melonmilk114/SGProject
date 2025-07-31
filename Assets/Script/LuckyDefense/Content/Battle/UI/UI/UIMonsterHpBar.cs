using System;
using Melon;
using UnityEngine;

namespace LuckyDefense
{
    public class UIMonsterHpBar : GameElement, IObjectPoolUnit
    {
        public UIGameImage uiBack;
        public UIGameImage uiFront;
        
        private Transform _target;
        private Vector3 _offset = new Vector3(0, 0.6f, 0); // 머리 위 위치

        public Action<UIMonsterHpBar> onObjectDestroy = null;
        
        public void OnPoolDequeue()
        {
            uiFront.fillAmount = 1f;
        }

        public void OnPoolEnqueue()
        {
            uiFront.fillAmount = 1f;
        }
        
        public void SetHpRatio(float inRatio)
        {
            uiFront.fillAmount = inRatio;
        }

        public void OnObjectDestroy()
        {
            onObjectDestroy?.Invoke(this);
        }

        public void AttachTarget(Transform target)
        {
            _target = target;
        }

        void LateUpdate()
        {
            if (_target == null) return;

            transform.position = _target.position + _offset;
            transform.forward = Camera.main.transform.forward; // UI 고정 방향 유지
        }

        
    }
}