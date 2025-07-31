using GridHeroes.Battle;
using Melon;
using UnityEngine;

namespace GridHeroes.UI
{
    public class BattleInGameCanvas : GameElement
    {
        public Transform hpBarRoot;
        public UIHpBar uiHpBarPrefab;
        
        public UITurnMarker uiTurnMarker;
        
        public UIHpBar CreateHpBar(UnitObject inUnit)
        {
            var newHpBar = Instantiate(uiHpBarPrefab, hpBarRoot);
            newHpBar.SetTargetUnit(inUnit);
            return newHpBar;
        }

        public void SetTurnUnit(UnitObject inUnit)
        {
            uiTurnMarker.SetTargetUnit(inUnit);
        }
    }
}