using GridHeroes.Battle;
using Melon;
using UnityEngine;

namespace GridHeroes
{
    public class UIHpBar : GameElement
    {
        public UIGameProgressBar uiProgressBar;

        private UnitObject _targetUnitObject = null;
        public Vector3 offsetPos = Vector3.zero;
        
        public void SetHpRatio(float inRatio)
        {
            uiProgressBar.progressValue = inRatio;
        }

        public void SetTargetUnit(UnitObject inUnitObject)
        {
            _targetUnitObject = inUnitObject;
            
            UpdatePos();
        }

        public void UpdatePos()
        {
            transform.position = _targetUnitObject.transform.position + offsetPos;
            transform.forward = Camera.main.transform.forward; // UI 고정 방향 유지
        }

        public void UpdateUI()
        {
            var hp = _targetUnitObject.GetHp();
            float ratio = (hp.Value > 0) ? (float)hp.Key / hp.Value : 0f;
            SetHpRatio(ratio);
        }
        
        
        void LateUpdate()
        {
            if (_targetUnitObject == null) return;
            UpdatePos();
        }
    }
}