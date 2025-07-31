using System.Collections;

namespace GridHeroes.Battle
{
    public class TrapTileObject : TileObject
    {
        public MoveStopStatusEffect statusEffect;
        
        public override IEnumerator OnTileStepped(UnitObject inUnit)
        {
            yield return inUnit.PlayAnimation(UNIT_ANIM_TYPE.HIT);

            // 맞은 캐릭터 처리
            if (inUnit != null)
            {
                if (statusEffect != null)
                {
                    var newStatusEffect = Instantiate(statusEffect);
                    yield return inUnit.AddStatusEffect(newStatusEffect);
                }
            }
            
            inUnit.PlayIdleAnimation();
        }
    }
}