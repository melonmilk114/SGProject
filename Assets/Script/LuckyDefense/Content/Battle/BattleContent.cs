using System;
using System.Collections.Generic;
using Melon;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace LuckyDefense
{
    public partial class BattleContent : Melon.Content
    {
        public class ContentData
        {
            public long stageSn;
        }

        public BattleTowerManager battleTowerManager;
        public BattleMissileManager battleMissileManager;
        public BattleMonsterManager battleMonsterManager;
        public BattleStageManager battleStageManager;

        public BattleService battleService;
        
        public BattleContentInGameCanvas inGameView;
        public BattleContentOutGameCanvas outGameView;

        public long stageSn = 0;
        public float battleStartElapsedTime = 0f;
        public bool isBattlePlaying = false;

        public override void OnAwakeFunc()
        {
            base.OnAwakeFunc();
            
            battleMonsterManager.AwakeContent();
            battleMonsterManager.onMissileDestroy = OnMissileDestroy;
            battleMonsterManager.onHpBarCreate = () => inGameView.CreateMonsterHpBar();
            battleMonsterManager.battleService = battleService;
            
            battleTowerManager.onCreateMissile = CreateMissile;
            battleTowerManager.onOpenTowerSelectMenu = OpenTowerSelectMenu;
            battleTowerManager.onCloseTowerSelectMenu = CloseTowerSelectMenu;
            battleTowerManager.battleService = battleService;
            
            battleStageManager.onCreateMonster = CreateMonster;

            outGameView.towerCreateInterface = battleTowerManager;
            inGameView.towerSelectMenuInterface = battleTowerManager;
        }

        public override void InitContent()
        {
            base.InitContent();
            
            battleTowerManager.InitContent();
            battleStageManager.InitContent();
            battleMissileManager.InitContent();
            battleMonsterManager.InitContent();

            battleService.InitService();
        }

        public override Enum GetContentType()
        {
            return ContentManager.ContentType.BATTLE;
        }

        private void Update()
        {
            if (isBattlePlaying == false)
                return;
            
            battleStartElapsedTime += Time.deltaTime;
            battleTowerManager.UpdateContent(Time.deltaTime);
            battleMissileManager.UpdateContent(Time.deltaTime);
            battleMonsterManager.UpdateContent(Time.deltaTime);
            battleStageManager.UpdateContent(Time.deltaTime, battleStartElapsedTime);
        }

        public override void DoShowCheck(object inData, ActionResult inActionResult)
        {
            if (inData is ContentData contentData)
            {
                stageSn = contentData.stageSn;

                battleStageManager.InitContentManager(stageSn);
            }

            inActionResult.OnSuccess();
        }
        
        public override void DoPostShow(object inData = null, ActionResult inActionResult = null)
        {
            // MEMO : 게임 시작
            base.DoPostShow(inData, inActionResult);
            isBattlePlaying = true;
        }

        private void CreateMissile(TowerGroupObject inTowerGroup)
        {
            // 적 탐색
            var monster = battleMonsterManager.FindTargetsInRange(inTowerGroup);
            if (monster == null)
                return;

            battleMissileManager?.CreateMissile(inTowerGroup, monster);
        }
        
        private void OpenTowerSelectMenu(TowerGroupObject inTowerGroup)
        {
            // 타워 메뉴 Open
            inGameView?.ShowTowerSelectMenu(inTowerGroup);
            
        }
        
        private void CloseTowerSelectMenu()
        {
            // 타워 메뉴 Open
            inGameView?.HideTowerSelectMenu();
        }


        private void CreateMonster(long inSn)
        {
            battleMonsterManager?.CreateMonster(inSn);
        }

        public void OnMouseDown()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                // UI 위 클릭 → 콜라이더 클릭 무시
                return;
            }
            battleTowerManager.OnMouseDown();
        }

        public void OnMouseDrag()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                // UI 위 클릭 → 콜라이더 클릭 무시
                return;
            }
            battleTowerManager.OnMouseDrag();
        }

        public void OnMouseUp()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                // UI 위 클릭 → 콜라이더 클릭 무시
                return;
            }
            battleTowerManager.OnMouseUp();
        }

        public void OnMissileDestroy(MissileObject inMissile)
        {
            battleMissileManager?.MissileDestroy(inMissile);
        }
    }
}
