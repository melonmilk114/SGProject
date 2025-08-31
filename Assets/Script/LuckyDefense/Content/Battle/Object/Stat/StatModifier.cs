using Melon;

namespace LuckyDefense
{
    public class StatModifier : GameElement
    {
        // 스탯을 수정 하는 클래스
        // 어떤 스탯을 수정 할껀지
        // 증가 할껀지 감소 할껀지
        // 곱연산인지 덧연산인지
        
        public STAT_TYPE statType;
        public STAT_MODIFIER_TYPE modifierType;

        public long level; // 몇레벨에 얻는 스탯인지 (향후 레벨업시 스탯 재계산에 사용)
        public float value;

        public void SetData(long inLevel, STAT_TYPE inStatType, STAT_MODIFIER_TYPE inModifierType, float inValue)
        {
            level = inLevel;
            statType = inStatType;
            modifierType = inModifierType;
            value = inValue * 0.01f;
        }
        
        /// <summary>
        /// 스탯 값에 모디파이어를 적용해서 최종 결과 반환
        /// </summary>
        public float Calculate(float baseValue)
        {
            switch (modifierType)
            {
                case STAT_MODIFIER_TYPE.ADDITIVE:
                    // 덧연산은 퍼센트 합산 개념 → base × (1 + value)
                    return baseValue * (1f + value);

                case STAT_MODIFIER_TYPE.MULTIPLY:
                    // 곱연산은 그대로 배율 곱
                    return baseValue * value;

                case STAT_MODIFIER_TYPE.NONE:
                default:
                    return baseValue;
            }
        }
    }
}