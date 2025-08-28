using UnityEngine;
using UnityEngine.UI;

namespace Melon
{
    public class UIGameImage : Image, IGameElement
    {
        #region IGameElement

        // GameElement를 컴포넌트로 가짐
        private GameElement _gameElement;
        
        // IGameElement 메서드들을 GameElement에 위임
        public void OnAwakeFunc() => _gameElement?.OnAwakeFunc();
        public void OnStartFunc() => _gameElement?.OnStartFunc();
        public void OnEnableFunc() => _gameElement?.OnEnableFunc();
        public void OnDisableFunc() => _gameElement?.OnDisableFunc();
        public void OnDestroyFunc() => _gameElement?.OnDestroyFunc();
        
        public void DoPreShow(object inData = null) => _gameElement?.DoPreShow(inData);
        public void DoPostShow(object inData = null) => _gameElement?.DoPostShow(inData);
        public void DoShowCheck(object inData = null) => _gameElement?.DoShowCheck(inData);
        public void DoShow(object inData = null, ActionResult inActionResult = null) => _gameElement?.DoShow(inData, inActionResult);
        
        public void DoPreHide(object inData = null) => _gameElement?.DoPreHide(inData);
        public void DoPostHide(object inData = null) => _gameElement?.DoPostHide(inData);
        public void DoHideCheck(object inData = null) => _gameElement?.DoHideCheck(inData);
        public void DoHide(object inData = null, ActionResult inActionResult = null) => _gameElement?.DoHide(inData, inActionResult);
        
        public void Init() => _gameElement?.Init();

        #endregion

#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();
            Init_Editor();
        }
#endif
        public void Init_Editor()
        {
            // 기본 설정
            if (sprite == null)
            {
                sprite = Resources.Load<Sprite>("Sprite/square");
                raycastTarget = false;
            }
        }
    }
}