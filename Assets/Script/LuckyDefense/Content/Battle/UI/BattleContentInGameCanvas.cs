using System.Collections.Generic;
using Melon;
using UnityEngine;
using UnityEngine.Serialization;

namespace LuckyDefense
{
    public class BattleContentInGameCanvas : GameElement
    {
        public GameElement monsterHpBarParent;
        public UITowerSelectMenu uiTowerSelectMenu;
        
        public void InitCanvas(ITowerSelectMenu inTowerSelectMenu)
        {
            uiTowerSelectMenu.towerSelectMenu = inTowerSelectMenu;
            uiTowerSelectMenu.DoHideUI();
        }
        
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
            // MEMO : 타워 판매(제거)
            uiTowerSelectMenu.transform.position = inTowerGroup.transform.position;
            uiTowerSelectMenu.DoShowUI(new UITowerSelectMenu.ShowData()
            {
                selectTowerGroup = inTowerGroup,
            });
        }

        public void HideTowerSelectMenu()
        {
            uiTowerSelectMenu.DoHideUI();
        }
    }
}