using System.Collections;
using Melon;
using UnityEngine;

namespace GridHeroes.Battle
{
    public class TileObject : GameElement
    {
        public Vector2Int offsetCoord;
        public Vector3Int cubeCoord;
        
        public Vector2Int LeftOffsetPos => new Vector2Int(-1, 0) + offsetCoord;
        public Vector2Int RightOffsetPos => new Vector2Int(1, 0)  + offsetCoord;

        public Vector2Int UpLeftOffsetPos => (offsetCoord.y % 2 == 0 ? new Vector2Int(-1, 1) : new Vector2Int(0, 1)) + offsetCoord;
        public Vector2Int UpRightOffsetPos => (offsetCoord.y % 2 == 0 ? new Vector2Int(0, 1) : new Vector2Int(1, 1)) + offsetCoord;
        public Vector2Int DownLeftOffsetPos => (offsetCoord.y % 2 == 0 ? new Vector2Int(-1, -1) : new Vector2Int(0, -1)) + offsetCoord;
        public Vector2Int DownRightOffsetPos => (offsetCoord.y % 2 == 0 ? new Vector2Int(0, -1) : new Vector2Int(1, -1)) + offsetCoord;

        public TileAnimator tileAnimator;
        
        public BoxCollider2D collider2D;
        public SpriteRenderer tileSprite;

        public TileData tileData;

        private ITileSelect _tileSelect = null;
        public int skillTargetScore = 0;

        public virtual void SetData(TileData inData, ITileSelect inTitleSelect, Vector2Int inOffsetCoord)
        {
            tileData = inData;
            _tileSelect = inTitleSelect;
            SetOffsetCoord(inOffsetCoord);
        }
        
        public void SetOffsetCoord(Vector2Int offset)
        {
            offsetCoord = offset;
            
            cubeCoord.x = offsetCoord.x - offsetCoord.y / 2;
            cubeCoord.y = offsetCoord.y;
            cubeCoord.z = -cubeCoord.y - cubeCoord.x;
        }
        
        private void OnMouseDown()
        {
            _tileSelect?.OnTileSelect(this);
        }

        public virtual IEnumerator OnTileStepped(UnitObject inUnit)
        {
            // MEMO : 타일을 밝을때 유닛한테 스킬 이펙트를 붙여준다
            yield return null;
        }
        
        public void PlayIdleAnimation()
        {
            tileAnimator?.PlayIdle();
        }
    }
}