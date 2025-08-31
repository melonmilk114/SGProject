using System.Collections.Generic;
using Melon;
using Unity.VisualScripting;

namespace LuckyDefense
{
    public class StatObject : GameElement
    {
        public Dictionary<STAT_TYPE, float> baseStats = new Dictionary<STAT_TYPE, float>();
        public Dictionary<STAT_TYPE, float> calculateStats = new Dictionary<STAT_TYPE, float>();
        
        public List<StatModifier> statModifiers = new List<StatModifier>();
        
        public void InitStatObject(GameElement inObject, long inLevel, TowerTableDataItem inTowerData, List<TowerLevelTableDataItem> inLevelDataList)
        {
            baseStats.Clear();
            calculateStats.Clear();
 
            this.AllRemoveComponent<StatModifier>();
            statModifiers.Clear();

            // 스텟 오브젝트들을 생성해서 붙혀준다
            inLevelDataList.ForEach(inItem =>
            {
                var comp = this.AddComponent<StatModifier>();
                comp.SetData(inItem.level, inItem.stat_type_enum, inItem.stat_modifier_type_enum, inItem.stat_modifier_value);
                statModifiers.Add(comp);
            });

            // TODO : 수정이 필요함
            baseStats[STAT_TYPE.ATTACK] = inTowerData.attack;
            baseStats[STAT_TYPE.ATTACK_SPEED] = inTowerData.attack_speed;
            baseStats[STAT_TYPE.CRITICAL_RATE] = inTowerData.critical_rate;
            
            // 스텟 계산
            CalculateStats(inLevel);
        }

        public void CalculateStats(long inLevel)
        {
            foreach (var keyValue in baseStats)
            {
                var statType = keyValue.Key;
                var baseValue = keyValue.Value;
                statModifiers.ForEach(inItem =>
                {
                    if (inItem.statType == statType && inItem.level <= inLevel)
                    {
                        baseValue = inItem.Calculate(baseValue);
                    }
                });
                
                calculateStats[statType] = baseValue;
            }
        }
        
        public float GetStat(STAT_TYPE inStatType)
        {
            if (calculateStats.TryGetValue(inStatType, out var stat))
                return stat;
            
            return 0f;
        }
    }
}