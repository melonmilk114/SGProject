using System;
using System.Collections.Generic;
using GridHeroes.Battle;
using Melon;

namespace GridHeroes.UI
{
    public class BattleOutGameCanvas : GameElement
    {
        public UIBattleResult uiBattleResult;
        public UIActionPanel uiActionPanel;

        public void ShowActionPanel(UnitObject inUnit, List<UnitSkill> inSkillList, Action<UnitSkill> inOnClickSkill)
        {
            if (inUnit != null)
            {
                uiActionPanel.DoShow(new UIActionPanel.ShowData
                {
                    unitObject = inUnit,
                    skillList = inSkillList,
                    onClickSkill = inOnClickSkill
                });
            }
            else
            {
                uiActionPanel.DoHide();
            }
            
        }
        
        public void HideBattleResult()
        {
            uiBattleResult.DoHide();
        }
        public void ShowBattleResult(bool inIsWin)
        {
            uiBattleResult.DoShow();
            uiBattleResult.ShowBattleResult(inIsWin);
        }
    }
}