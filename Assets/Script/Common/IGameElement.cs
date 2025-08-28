using System;

namespace Melon
{
    public interface IGameElement
    {
        // 생명주기 메서드
        void OnAwakeFunc();
        void OnStartFunc();
        void OnEnableFunc();
        void OnDisableFunc();
        void OnDestroyFunc();
        
        // 표시/숨김 메서드
        void DoPreShow(object inData = null);
        void DoPostShow(object inData = null);
        void DoShowCheck(object inData = null);
        void DoShow(object inData = null, ActionResult inActionResult = null);
        
        void DoPreHide(object inData = null);
        void DoPostHide(object inData = null);
        void DoHideCheck(object inData = null);
        void DoHide(object inData = null, ActionResult inActionResult = null);
        
        // 초기화
        void Init();
    }
}