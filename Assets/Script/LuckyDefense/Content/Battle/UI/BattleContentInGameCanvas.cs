using System.Collections.Generic;
using LuckyDefense.Interface;
using Melon;
using UnityEngine;
using UnityEngine.Serialization;

namespace LuckyDefense
{
    public class BattleContentInGameCanvas : GameElement
    {
        public GameElement monsterHpBarParent;
        public UITowerSelectMenu uiTowerSelectMenu;

        public ITowerSelectMenu towerSelectMenuInterface = null;
        
        public UIMonsterHpBar CreateMonsterHpBar()
        {
            var rv = ObjectPoolManager.Instance.DequeuePool<UIMonsterHpBar>(monsterHpBarParent.gameObject);
            rv.onObjectDestroy = (inObj) =>
            {
                ObjectPoolManager.Instance.EnqueuePool(inObj);
            };
            return rv;
        }

        public void ShowTowerSelectMenu(TowerGroupObject inTowerGroup)
        {
            // MEMO : 타워 선택 메뉴
            // MEMO : 타워 업그레이드, 타워 판매(제거)
            uiTowerSelectMenu.transform.position = inTowerGroup.transform.position;
            uiTowerSelectMenu.DoShowUI(new UITowerSelectMenu.ShowData()
            {
                selectTowerGroup = inTowerGroup,
                towerSelectMenuInterface = towerSelectMenuInterface
            });
        }

        public void HideTowerSelectMenu()
        {
            uiTowerSelectMenu.DoHideUI();
        }
    }
}