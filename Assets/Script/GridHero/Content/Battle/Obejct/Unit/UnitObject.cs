using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Melon;
using UnityEngine;
using UnityEngine.Serialization;

namespace GridHeroes.Battle
{
    public class UnitObject : GameElement
    {
        public UnitAnimator unitAnimator;
        public SpriteRenderer unitSpriteRenderer; 

        public Vector2Int tileOffsetCoord;

        public Transform statModifierRoot;
        public Transform skillRoot;
        public Transform statusEffectRoot;

        public List<StatData> statDataList = new List<StatData>();

        public bool isAutoSkill = false;
        public UNIT_FACTION unitFaction = UNIT_FACTION.ALLY;

        public UIHpBar uiHpBar = null;
        
        public bool isTurnEnd = false;
        
        public IBattleContent battleContent;
        
        public override void OnDestroyFunc()
        {
            Destroy(uiHpBar.gameObject);
        }

        public void InitUnit(Vector2Int inOffsetCoord)
        {
            if (uiHpBar == null)
            {
                uiHpBar = battleContent.CreateHpBar(this);
                uiHpBar.UpdateUI();
            }
            SetTileOffsetCoord(inOffsetCoord);
        }

        public bool IsAutoSkillUse()
        {
            return isAutoSkill;
        }
        
        public void SetTileOffsetCoord(Vector2Int inOffsetCoord)
        {
            tileOffsetCoord = inOffsetCoord;
        }

        public void PlayLoopAnimation(UNIT_ANIM_TYPE inType)
        {
            unitAnimator.PlayLoop(inType);
        }
        public void PlayIdleAnimation()
        {
            unitAnimator.PlayIdle();
        }
        
        public IEnumerator PlayAnimation(UNIT_ANIM_TYPE inType)
        {
            yield return unitAnimator.Play(inType);
        }
        
        public void ChangeFacingDirection(Vector3 inDirection)
        {
            unitSpriteRenderer.flipX = transform.position.x > inDirection.x;
        }
        
        public void ChangeAttachTileOffsetCoord(TileObject inTile)
        {
            tileOffsetCoord = inTile.offsetCoord;
            transform.position = inTile.transform.position;
        }

        public UNIT_FACTION GetEnemyFaction()
        {
            switch (unitFaction)
            {
                case UNIT_FACTION.ALLY:
                    return UNIT_FACTION.ENEMY;
                case UNIT_FACTION.ENEMY:
                    return UNIT_FACTION.ALLY;
                case UNIT_FACTION.NEUTRAL:
                    return UNIT_FACTION.NONE;
                    break;
            }

            return UNIT_FACTION.NONE;

        }

        public IEnumerator AddStatusEffect(StatusEffect inStatusEffect)
        {
            inStatusEffect.transform.SetParent(statusEffectRoot);

            yield return inStatusEffect.Co_ApplyStatusEffect(this);
        }

        public IEnumerator AddStatModifier(UnitStatModifier inStatModifier)
        {
            inStatModifier.transform.SetParent(statModifierRoot);
            
            yield return inStatModifier.Co_ApplyStatModifier();
        }
        
        public int GetStatValue(STAT_TYPE inStatType)
        {
            var findStat = statDataList.Find(inFindItem => inFindItem.statType == inStatType);
            if (findStat == null)
                return 0;

            int baseValue = findStat.value;

            int additiveValue = 0;
            int multipleValue = 0;

            var modifiers = statModifierRoot.GetComponentsInChildren<UnitStatModifier>();

            foreach (var modifier in modifiers)
            {
                if (modifier.isEqualStat(inStatType) == false)
                    continue;

                if (modifier.statModifierData.statModifierType == STAT_MODIFIER_TYPE.ADDITIVE)
                    additiveValue += modifier.statModifierData.value;
                else
                    multipleValue += modifier.statModifierData.value;
            }

            int finalValue = baseValue + additiveValue + baseValue * multipleValue / 100;
            return finalValue;
        }

        public void SetStatValue(STAT_TYPE inStatType, int inValue)
        {
            var findStat = statDataList.Find(inFindItem => inFindItem.statType == inStatType);
            if (findStat == null)
                return;

            if (inValue < 0)
                inValue = 0;
            
            findStat.value = inValue;
        }

        public bool IsTurnAvailable()
        {
            var currHP = GetStatValue(STAT_TYPE.CURR_HP);
            if (currHP <= 0)
                return false;
            
            // 스킬 사용이 가능 여부를 체크 해야함
            var currAP = GetStatValue(STAT_TYPE.CURR_AP);
            if (currAP <= 0)
                return false;

            return IsAlive() && isTurnEnd == false;
        }

        public bool IsAlive()
        {
            var currHP = GetStatValue(STAT_TYPE.CURR_HP);
            if (currHP <= 0)
                return false;

            return true;
        }

        public KeyValuePair<int, int> GetHp()
        {
            return new KeyValuePair<int, int>(GetStatValue(STAT_TYPE.CURR_HP), GetStatValue(STAT_TYPE.MAX_HP));
        }

        public IEnumerator TakeDamage(int inDamage)
        {
            var currHP = GetStatValue(STAT_TYPE.CURR_HP);
            SetStatValue(STAT_TYPE.CURR_HP, currHP - inDamage);
            currHP = GetStatValue(STAT_TYPE.CURR_HP);

            yield return PlayAnimation(UNIT_ANIM_TYPE.HIT);
            PlayIdleAnimation();

            // HP UI 수정;
            uiHpBar.UpdateUI();

            if (currHP <= 0)
            {
                yield return DeadRoutine();
            }
        }
        
        public IEnumerator TakeHeal(int inHeal)
        {
            var currHP = GetStatValue(STAT_TYPE.CURR_HP);
            var maxHP = GetStatValue(STAT_TYPE.MAX_HP);
            if(maxHP >= currHP + inHeal)
                SetStatValue(STAT_TYPE.CURR_HP, currHP + inHeal);
            else
                SetStatValue(STAT_TYPE.CURR_HP, maxHP);

            yield return PlayAnimation(UNIT_ANIM_TYPE.HIT);
            PlayIdleAnimation();

            // HP UI 수정;
            uiHpBar.UpdateUI();
        }
        
        public IEnumerator DeadRoutine()
        {
            yield return PlayAnimation(UNIT_ANIM_TYPE.DIE);
        }

        public IEnumerator Co_TurnStart()
        {
            var effects = statusEffectRoot.GetComponentsInChildren<StatusEffect>();

            yield return this.WhenAll(effects.Select(effect => effect.Co_TurnStartRoutine(this)).ToArray());
        }
        public IEnumerator Co_TurnEnd()
        {
            var effects = statusEffectRoot.GetComponentsInChildren<StatusEffect>();

            yield return this.WhenAll(effects.Select(effect => effect.Co_TurnEndRoutine(this)).ToArray());
        }
        public IEnumerator Co_RoundStart()
        {
            // 턴 스킵 해제
            isTurnEnd = false;
            
            // AP 전부 회복 
            var maxAP = GetStatValue(STAT_TYPE.MAX_AP);
            SetStatValue(STAT_TYPE.CURR_AP, maxAP);
            
            // 스킬 쿨다운 회복
            var skills = battleContent.GetSkills(this);
            skills.ForEach(inForItem => inForItem.RecoverCooldown());
            
            var effects = statusEffectRoot.GetComponentsInChildren<StatusEffect>();

            yield return this.WhenAll(effects.Select(effect => effect.Co_RoundStartRoutine(this)).ToArray());
        }
        
        public IEnumerator Co_RoundEnd()
        {
            var effects = statusEffectRoot.GetComponentsInChildren<StatusEffect>();

            yield return this.WhenAll(effects.Select(effect => effect.Co_RoundEndRoutine(this)).ToArray());
            
            var statModifiers = statusEffectRoot.GetComponentsInChildren<UnitStatModifier>();

            yield return this.WhenAll(statModifiers.Select(effect => effect.Co_RoundEndRoutine()).ToArray());
        }
        
        #region Context Menu
        [ContextMenu("Make Stat")]
        public void MakeStat()
        {
            int statCount = Enum.GetValues(typeof(STAT_TYPE)).Length;
            for (int i = 0; i < statCount; i++)
            {
                var stat = new StatData() { statType = (STAT_TYPE)i, value = 0 };
                statDataList.Add(stat);
            }
        }
        
        [ContextMenu("Update Stat")]
        public void UpdateStat()
        {
            statDataList.ForEach(inForItem =>
            {
                inForItem.value = GetStatValue(inForItem.statType);
            });
        }

        #endregion
    }
}