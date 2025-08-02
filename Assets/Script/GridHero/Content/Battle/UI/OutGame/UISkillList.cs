using System;
using System.Collections.Generic;
using GridHero.Battle;
using Melon;

namespace GridHero
{
    public class UISkillList : GameElement
    {
        public List<UISkillButtonSlot> skillButtonSlots = new List<UISkillButtonSlot>();

        private List<UnitSkill> _skillList = new List<UnitSkill>();
        
        public void SetSkillList(int inCurrAp, List<UnitSkill> inSkillList, Action<UnitSkill> inOnClickSkill)
        {
            _skillList = inSkillList;
            
            skillButtonSlots.ForEach(inForItem => inForItem.DoHide());

            for (int idx = 0; idx < _skillList.Count; ++idx)
            {
                skillButtonSlots[idx].DoShow();
                skillButtonSlots[idx].SetUnitSkill(inCurrAp, _skillList[idx], inOnClickSkill);
            }

            UpdateUI();
        }

        public void UpdateUI()
        {
            skillButtonSlots.ForEach(inForItem => inForItem.UpdateUI());
        }
    }
}