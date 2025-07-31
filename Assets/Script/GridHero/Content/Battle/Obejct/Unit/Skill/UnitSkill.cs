using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GridHeroes.Battle
{
    public abstract class UnitSkill : MonoBehaviour
    {
        // 스킬 데이터
        public SkillData skillData;
        // 스킳 쿨다운
        public int coolDown;
        // 스킬 사용자
        public UnitObject caster;
        
        // 타겟이 가능한 타일 목록
        protected List<TileObject> targetAvailableTiles = new List<TileObject>();
        
        // 선택한 타겟 타일
        protected TileObject targetSelectTile = null;
        
        // 스킬 적용 받는 타일 목룍
        protected List<TileObject> effectTiles = new List<TileObject>();
        
        // 이동 타일 목록
        protected List<TileObject> moveTiles = new List<TileObject>();
        
        // 타겟팅이 가능한 타일 목록 반환
        public abstract List<TileObject> FindTargetAvailableTiles();
        
        // 자동으로 타겟 타일 설정
        public virtual void AutoSkillTargetTile()
        {
            targetSelectTile = targetAvailableTiles.OrderByDescending(tile => tile.skillTargetScore).First();
        }
        
        // 스킬 적용 받는 타일 목록
        public virtual List<TileObject> FindEffectTiles()
        {
            return new List<TileObject>() { targetSelectTile };
        }
        
        // 이동 타일 목록 반환
        // 시전자위치에서 타겟 타일 까지 탐색
        public virtual List<TileObject> FindMoveTiles()
        {
            return new List<TileObject>() {targetSelectTile};
        }
        
        // 스킬 사용 가능한 타일의 점수 업데이트
        public virtual void UpdateSkillTargetScore()
        {
            
        }
        
        // 스킬 사용 결정
        private bool _finalTargetDetermined = false;
        
        // 실제 스킬 발동
        public abstract IEnumerator Co_SkillExecuteRoutine();
        
        //// 스킬 매니저로 이동 시켜야함
        public IBattleContent battleContent;
        //// 스킬 매니저로 이동 시켜야함
        
        public virtual void ResetSkill()
        {
            targetSelectTile = null;
            
            targetAvailableTiles.Clear();
            effectTiles.Clear();
            moveTiles.Clear();

            _finalTargetDetermined = false;
        }
        
        
        // 스킬 사용 타일 클릭
        public void OnClickTargetTile(TileObject inTile)
        {
            if (targetSelectTile == null || targetSelectTile != inTile)
            {
                targetSelectTile = inTile;
                targetAvailableTiles = FindTargetAvailableTiles();
                effectTiles = FindEffectTiles();
                moveTiles = FindMoveTiles();

                UpdateSkillTargetTile();
            }
            else
            {
                targetAvailableTiles = FindTargetAvailableTiles();
                effectTiles = FindEffectTiles();
                moveTiles = FindMoveTiles();

                _finalTargetDetermined = true;
            }
        }
        
        // 스킬을 사용 해서 쿨다운이 시작됨
        public void UseSkill()
        {
            coolDown = skillData.coolDown;
        }
        
        // 쿨다운 회복
        public void RecoverCooldown()
        {
            coolDown--;
            if (coolDown < 0)
                coolDown = 0;
        }
        
        // 스킬 선택 루틴
        public IEnumerator Co_SkillSelectRoutine()
        {
            // 데이터 초기화
            ResetSkill();
            
            // 스킬 사용 영역 지정
            targetAvailableTiles = FindTargetAvailableTiles();
            
            // 스킬 사용 가능한 타일 표시
            var allTiles = battleContent?.GetAllTiles();
            var normalTiles = allTiles.Except(targetAvailableTiles).ToList();
            battleContent.ChangeTileColor(normalTiles, Color.white);
            battleContent.ChangeTileColor(targetAvailableTiles, Color.green);
        
            if (caster.IsAutoSkillUse())
            {
                // 자동 스킬 사용인 경우
                UpdateSkillTargetScore();
                AutoSkillTargetTile();
                
                effectTiles = FindEffectTiles();
                moveTiles = FindMoveTiles();
                
                normalTiles = allTiles.Except(effectTiles).Except(moveTiles).Except(targetAvailableTiles)
                    .Where(x => x != targetSelectTile).ToList();
                battleContent.ChangeTileColor(normalTiles, Color.white);
                battleContent.ChangeTileColor(targetAvailableTiles, Color.green);
                battleContent.ChangeTileColor(moveTiles, Color.yellow);
                battleContent.ChangeTileColor(targetSelectTile, Color.blue);
                battleContent.ChangeTileColor(effectTiles, Color.red);
            }
            else
            {
                // 플레이어가 스킬 사용 타일을 선택할 때까지 대기
                yield return new WaitUntil(() => targetSelectTile != null);
                
                // 스킬 사용 예측 타일 표시
                UpdateSkillTargetTile();
                
                // 플레이어가 스킬 사용 타일을 선택할 때까지 대기
                yield return new WaitUntil(() => _finalTargetDetermined);
                
                // 스킬 사용 타일 표시
                normalTiles = allTiles.Except(effectTiles).Except(moveTiles)
                    .Where(x => x != targetSelectTile).ToList();
                battleContent.ChangeTileColor(normalTiles, Color.white);
                battleContent.ChangeTileColor(moveTiles, Color.yellow);
                battleContent.ChangeTileColor(targetSelectTile, Color.blue);
                battleContent.ChangeTileColor(effectTiles, Color.red);
            }
            
            yield return null;
        }

        public void UpdateSkillTargetTile()
        {
            var normalTiles = battleContent?.GetAllTiles().Except(effectTiles).Except(moveTiles).Except(targetAvailableTiles)
                .Where(x => x != targetSelectTile).ToList();
            battleContent.ChangeTileColor(normalTiles, Color.white);
            battleContent.ChangeTileColor(targetAvailableTiles, Color.green);
            battleContent.ChangeTileColor(moveTiles, Color.yellow);
            battleContent.ChangeTileColor(targetSelectTile, Color.blue);
            battleContent.ChangeTileColor(effectTiles, Color.red);
        }
        
        public void UpdateAllNormalTile()
        {
            var normalTiles = battleContent?.GetAllTiles();
            battleContent.ChangeTileColor(normalTiles, Color.white);
        }
    }
}