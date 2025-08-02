using System;
using GridHero.Battle;
using Melon;

namespace GridHero
{
    public class UISkillButtonSlot : GameElement
    {
        public UIGameButton button;
        public UIGameLabel uiSkillName;
        public UIGameLabel uiSkillApCost;
        public UIGameLabel uiSkillCooldown;

        private UnitSkill _unitSkill = null;
        private Action<UnitSkill> _onClickSkill = null;
        
        public override void OnAwakeFunc()
        {
            button.SetClickAction(() =>
            {
                if(_unitSkill != null)
                    _onClickSkill?.Invoke(_unitSkill);
            });
        }
        
        public void SetUnitSkill(int inCurrAp, UnitSkill inSkill, Action<UnitSkill> inOnClickSkill)
        {
            _unitSkill = inSkill;
            _onClickSkill = inOnClickSkill;
            
            button.interactable = inSkill.skillData.apCost <= inCurrAp && inSkill.coolDown <= 0;

            UpdateUI();
        }

        public void UpdateUI()
        {
            if (_unitSkill == null)
                return;
            
            uiSkillName.SetText($"{_unitSkill.skillData.name}");
            uiSkillApCost.SetText($"ApCost : {_unitSkill.skillData.apCost.ToString()}");
            uiSkillCooldown.SetText($"CoolDown : {_unitSkill.coolDown.ToString()}");
        }
    }
}