using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GridHero.Battle
{
    public class BattleSkillManager : MonoBehaviour
    {
        private Dictionary<UnitObject, List<UnitSkill>> _skillDict = new Dictionary<UnitObject, List<UnitSkill>>();
        public IBattleContent battleContent; 
        public void BattleStart()
        {
            _skillDict.Clear();
            // 생성된 유닛의 스킬에 매니저를 주입 시긴다.
            var units = battleContent?.GetAllUnits();
            units.ForEach(inForItem =>
            {
                var skills = inForItem.skillRoot.GetComponentsInChildren<UnitSkill>().ToList();
                //var skills = inForItem.GetSkills();
                _skillDict.TryAdd(inForItem, new List<UnitSkill>());
                _skillDict[inForItem].AddRange(skills);
                
                skills.ForEach(inForItem_1 =>
                {
                    inForItem_1.caster = inForItem;
                    inForItem_1.battleContent = battleContent;
                });
            });
        }
        
        public List<UnitSkill> GetSkills(UnitObject inUnitObject)
        {
            return _skillDict.TryGetValue(inUnitObject, out var list) ? list : new List<UnitSkill>();
        }
        
        public List<UnitSkill> GetCanUseSkills(UnitObject inUnitObject)
        {
            var skills = GetSkills(inUnitObject);
            int currAp = inUnitObject.GetStatValue(STAT_TYPE.CURR_AP);
            
            return skills.FindAll(inFindItem => inFindItem.skillData.apCost <= currAp && inFindItem.coolDown <= 0);
        }
        
        public List<UnitSkill> GetCanUseSkills(UnitObject inUnitObject, SKILL_TYPE inSkillType)
        {
            var skills = GetCanUseSkills(inUnitObject);
            
            return skills.FindAll(inFindItem => inFindItem.skillData.skillType == inSkillType);
        }
        
        public void UseSkill(UnitSkill inSkill)
        {
            if (inSkill != null)
            {
                var currAP = inSkill.caster.GetStatValue(STAT_TYPE.CURR_AP);
                inSkill.caster.SetStatValue(STAT_TYPE.CURR_AP, currAP - inSkill.skillData.apCost);
                inSkill.UseSkill();
            }
        }
        
    }
}