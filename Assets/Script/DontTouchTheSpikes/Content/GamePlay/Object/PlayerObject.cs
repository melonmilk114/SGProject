using System;
using Melon;
using UnityEngine;

namespace DontTouchTheSpikes
{
    public class PlayerObject : GameElement
    {
        public Action<Collider2D> onPlayerTriggerEnter = null;
        
        private	float				moveSpeed = 5f;		// 이동 속도
        private	float				jumpForce = 15f;		// 점프 힘
        
        private	Rigidbody2D _rigidbody;

        public override void OnAwakeFunc()
        {
            base.OnAwakeFunc();
            _rigidbody = GetComponent<Rigidbody2D>();
        }
        
        public void GameReady()
        {
            // 플레이어 준비
            _rigidbody.gravityScale = 0;
            _rigidbody.velocity = Vector2.zero;
            transform.position = new Vector3(0, -1.69f, 0);
        }
        
        public void GameStart()
        {
            // 플레이어 시작
            _rigidbody.isKinematic = false;
            _rigidbody.gravityScale = 5;
            _rigidbody.velocity	 = new Vector2(moveSpeed, jumpForce);
            
            transform.position = Vector3.zero;
        }
        
        public void GameOver()
        {
            // 게임오버
            _rigidbody.gravityScale = 0;
            _rigidbody.velocity = Vector2.zero;
            //transform.position = Vector3.zero;
        }
        
        
        public PLAYER_MOVE_DIR GetHorizontalDirection()
        {
            float x = _rigidbody.velocity.x;

            if (Mathf.Approximately(x, 0f))
                return PLAYER_MOVE_DIR.NONE;
            else if (x > 0f)
                return PLAYER_MOVE_DIR.RIGHT;
            else
                return PLAYER_MOVE_DIR.LEFT;
        }

        public void ReverseHorizontalDirection()
        {
            _rigidbody.velocity = new Vector2(-Mathf.Sign(_rigidbody.velocity.x) * moveSpeed, _rigidbody.velocity.y);
        }

        public void Jump()
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, jumpForce);
        }
        
        private void OnTriggerEnter2D(Collider2D collision)
        {
            onPlayerTriggerEnter?.Invoke(collision);
        }
    }
}