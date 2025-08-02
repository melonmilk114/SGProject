using GridHero.Battle;
using Melon;
using UnityEngine;
using UnityEngine.Rendering;

namespace GridHero
{
    public class GHFramework : GameFramework
    {
        public override void InitFramework()
        {
            base.InitFramework();
            
            // Transparency Sort Mode 변경
            GraphicsSettings.transparencySortMode = TransparencySortMode.CustomAxis;

            // Transparency Sort Axis 설정 (예: Y축 우선 정렬)
            GraphicsSettings.transparencySortAxis = new Vector3(0, 1, 0);
        }
    }
}