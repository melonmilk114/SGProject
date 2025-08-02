using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace GridHero.Battle
{
    public class BattleStateManager : MonoBehaviour
    , IBattleStateManager
    {
        private Dictionary<BATTLE_STATE, BattleState> _battleStateDict = new Dictionary<BATTLE_STATE, BattleState>();
        private BattleState _currentState;
        
        private Dictionary<BATTLE_EFFECT_STATE, BattleEffectState> _battleEffectStateDict = new Dictionary<BATTLE_EFFECT_STATE, BattleEffectState>();
        private BattleEffectState _currentEffectState;
        
        public List<BattleCheckCondition> winCheckConditions = new List<BattleCheckCondition>();
        public List<BattleCheckCondition> loseCheckConditions = new List<BattleCheckCondition>();

        public IBattleContent battleContent;
        
        public void InitManager()
        {
            _battleStateDict.Clear();
            _battleStateDict.Add(BATTLE_STATE.BATTLE_START, this.GetOrAddComponent<BattleStartState>());
            _battleStateDict.Add(BATTLE_STATE.ROUND_START, this.GetOrAddComponent<RoundStartState>());
            _battleStateDict.Add(BATTLE_STATE.DETERMINE_TURN, this.GetOrAddComponent<DetermineTurnState>());
            _battleStateDict.Add(BATTLE_STATE.TURN_START, this.GetOrAddComponent<TurnStartState>());
            _battleStateDict.Add(BATTLE_STATE.SELECT_SKILL, this.GetOrAddComponent<SelectSkillState>());
            _battleStateDict.Add(BATTLE_STATE.EXECUTE_SKILL, this.GetOrAddComponent<ExecuteSkillState>());
            _battleStateDict.Add(BATTLE_STATE.TURN_END, this.GetOrAddComponent<TurnEndState>());
            _battleStateDict.Add(BATTLE_STATE.ROUND_END, this.GetOrAddComponent<RoundEndState>());
            _battleStateDict.Add(BATTLE_STATE.BATTLE_END, this.GetOrAddComponent<BattleEndState>());
            
            foreach (var battleState in _battleStateDict.Values)
            {
                battleState.ClearBattleStateDelegates();
                battleState.battleStateManager = this;
                battleState.battleContent = battleContent;
            }
            
            _battleEffectStateDict.Clear();
            _battleEffectStateDict.Add(BATTLE_EFFECT_STATE.WIN, this.GetOrAddComponent<WinEffectState>());
            _battleEffectStateDict.Add(BATTLE_EFFECT_STATE.LOSE, this.GetOrAddComponent<LoseEffectState>());
            
            foreach (var battleState in _battleEffectStateDict.Values)
            {
                battleState.battleStateManager = this;
                battleState.battleContent = battleContent;
            }
            
            foreach (var checkCondition in winCheckConditions)
            {
                checkCondition.battleContent = battleContent;
            }
            
            foreach (var checkCondition in loseCheckConditions)
            {
                checkCondition.battleContent = battleContent;
            }
        }
        public void ChangeBattleState(BATTLE_STATE inState)
        {
            _currentState = _battleStateDict[inState];
            Debug.Log($"ChangeToState: {inState}");
            StartCoroutine(Co_StateRoutine());
        }
        
        public void AddBattleStateDelegate(BATTLE_STATE inState, Func<IEnumerator> inDelegate)
        {
            if (_battleStateDict.TryGetValue(inState, out var battleState))
            {
                battleState.AddBattleStateDelegate(inDelegate);
            }
            else
            {
                Debug.LogError($"Battle state {inState} not found.");
            }
        }

        public void RemoveBattleStateDelegate(BATTLE_STATE inState, Func<IEnumerator> inDelegate)
        {
            if (_battleStateDict.TryGetValue(inState, out var battleState))
            {
                battleState.RemoveBattleStateDelegate(inDelegate);
            }
            else
            {
                Debug.LogError($"Battle state {inState} not found.");
            }
        }

        public void ClearBattleStateDelegate(BATTLE_STATE inState)
        {
            if (_battleStateDict.TryGetValue(inState, out var battleState))
            {
                battleState.ClearBattleStateDelegates();
            }
            else
            {
                Debug.LogError($"Battle state {inState} not found.");
            }
        }
        
        public void ChangeBattleEffectState(BATTLE_EFFECT_STATE inState)
        {
            _currentEffectState = _battleEffectStateDict[inState];
            Debug.Log($"ChangeBattleEffectState: {inState}");
            StartCoroutine(_currentEffectState.Co_StateRoutine());
        }

        public IEnumerator Co_StateRoutine()
        {
            if (CheckWinCondition())
            {
                ChangeBattleEffectState(BATTLE_EFFECT_STATE.WIN);
                yield break;
            }

            if (CheckLoseCondition())
            {
                ChangeBattleEffectState(BATTLE_EFFECT_STATE.LOSE);
                yield break;
            }
            
            yield return _currentState.Co_StateRoutine();
        }

        public bool CheckWinCondition()
        {
            foreach (var condition in winCheckConditions)
            {
                if (condition.CheckCondition())
                {
                    return true;
                }
            }
            return false;
        }

        public bool CheckLoseCondition()
        {
            foreach (var condition in loseCheckConditions)
            {
                if (condition.CheckCondition())
                {
                    return true;
                }
            }
            return false;
        }

        public void BattleStart()
        {
            foreach (var battleState in _battleStateDict.Values)
            {
                battleState.ClearBattleStateDelegates();
            }
            
            ChangeBattleState(BATTLE_STATE.BATTLE_START);
        }

       

        public bool IsCheckGameWin()
        {
            return true;
        }
    }
}