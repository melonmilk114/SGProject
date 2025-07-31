using System;
using System.Collections.Generic;
using GridHeroes.Battle;
using Melon;

namespace GridHeroes
{
    public class UIActionPanel : GameElement
    {
        public class ShowData
        {
            public UnitObject unitObject;
            public List<UnitSkill> skillList;

            public Action<UnitSkill> onClickSkill;
        }
        public UIGameLabel uiName;
        public UIGameLabel uiCurrAp;
        public UIGameLabel uiCurrHp;
        
        public UISkillList uiSkillList;

        public UIGameButton uiTurnSkip;
        
        public UnitObject unitObject;
        public List<UnitSkill> skillList;
        public Action<UnitSkill> onClickSkill;

        public override void OnAwakeFunc()
        {
            uiTurnSkip.SetClickAction(() =>
            {
                onClickSkill?.Invoke(null);
            });
        }

        public override void DoPreShow(object inData = null)
        {
            if (inData is ShowData showData)
            {
                unitObject = showData.unitObject;
                skillList = showData.skillList;
                onClickSkill = showData.onClickSkill;
            }
        }
        
        public override void DoPostShow(object inData = null)
        {
            uiName.SetText(unitObject.name);
            uiCurrAp.SetText($"현재 Ap : {unitObject.GetStatValue(STAT_TYPE.CURR_AP)}");
            uiCurrHp.SetText($"현재 Hp : {unitObject.GetStatValue(STAT_TYPE.CURR_HP)}");
            
            uiSkillList.SetSkillList(unitObject.GetStatValue(STAT_TYPE.CURR_AP), skillList, onClickSkill);
            
        }
        
        
    }
}