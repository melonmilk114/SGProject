namespace GridHero
{
    public enum UNIT_ANIM_TYPE
    {
        IDLE = 0,
        MOVE,
        HIT,
        DIE,
        ATTACK,
        SKILL_1,
        SKILL_2
    }
    
    public enum TILE_ANIM_TYPE
    {
        IDLE = 0,
    }

    public enum UNIT_FACTION
    {
        NONE,
        ALLY,
        ENEMY,
        NEUTRAL,
    }


    public enum STAT_TYPE
    {
        MAX_HP = 0,
        CURR_HP,
        ATK,
        SPD,
        INITIATIVE, // 높을수록 선공
        MAX_AP, // 최대 행동력
        CURR_AP, // 현재 행동력
    }
    
    public enum STAT_MODIFIER_TYPE
    {
        ADDITIVE = 0, // 단순히 더하기
        MULTIPLE, // 곱하기
    }
    
    public enum BATTLE_STATE
    {
        NONE = 0,
        BATTLE_START,
        ROUND_START,
        DETERMINE_TURN, // 누구 턴일지 고르기
        TURN_START, 
        SELECT_SKILL,
        EXECUTE_SKILL,
        TURN_END,
        ROUND_END,
        BATTLE_END,
    }
    
    public enum BATTLE_EFFECT_STATE
    {
        NONE = 0,
        WIN,
        LOSE,
    }

    public enum SKILL_TYPE
    {
        NONE,
        ATTACK,
        BUFF,
        DEBUFF,
        MOVEMENT,
    }

    public enum TILE_TYPE
    {
        NONE,
        NORMAL,
        BUFF,
        DEBUFF,
    }
}