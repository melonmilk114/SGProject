using System;
using GridHeroes.Battle;
using Melon;
using UnityEngine;

namespace GridHeroes
{
    public class UITurnMarker : GameElement
    {
        public UIGameImage uiMarker;

        private UnitObject _targetUnitObject = null;
        public Vector3 offsetPos = Vector3.zero;

        public void SetTargetUnit(UnitObject inUnitObject)
        {
            _targetUnitObject = inUnitObject;
            if (inUnitObject == null)
            {
                DoHide();
                return;
            }
            
            DoShow();
                
            switch (_targetUnitObject.unitFaction)
            {
                case UNIT_FACTION.ALLY:
                    uiMarker.color = Color.green;
                    break;
                case UNIT_FACTION.ENEMY:
                    uiMarker.color = Color.red;
                    break;
                case UNIT_FACTION.NEUTRAL:
                    uiMarker.color = Color.blue;
                    break;
                default:
                    uiMarker.color = Color.white;
                    break;
            };
            
            UpdatePos();
        }

        public void UpdatePos()
        {
            transform.position = _targetUnitObject.transform.position + offsetPos;
            transform.forward = Camera.main.transform.forward; // UI 고정 방향 유지
        }
        
        
        void LateUpdate()
        {
            if (_targetUnitObject == null) return;
            UpdatePos();
        }
    }
}