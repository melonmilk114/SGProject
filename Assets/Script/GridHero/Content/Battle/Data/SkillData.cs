namespace GridHero.Battle
{
#if UNITY_EDITOR
    [System.Serializable]
#endif
    
    public class SkillData
    {
        public string name;
        public int apCost = 0;
        public int coolDown = 0;
        public SKILL_TYPE skillType = SKILL_TYPE.NONE;
    }
}