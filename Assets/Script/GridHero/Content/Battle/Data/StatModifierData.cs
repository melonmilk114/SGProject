namespace GridHeroes.Battle
{
#if UNITY_EDITOR
    [System.Serializable]
#endif
    public class StatModifierData
    {
        public int value;
        public STAT_TYPE statType;
        public STAT_MODIFIER_TYPE statModifierType;
        public int duration;
    }
}