using System.Collections;
using Melon;
using UnityEngine;

namespace CubeStack
{
    public class StackCube : GameElement
    {
        private MeshRenderer meshRenderer;

        public override void OnAwakeFunc()
        {
            base.OnAwakeFunc();
            
            meshRenderer = GetComponent<MeshRenderer>();
        }

        public void SetColor(Color inColor)
        {
            if (meshRenderer != null)
                meshRenderer.material.color = inColor;
        }

        public Color GetColor()
        {
            return meshRenderer != null ? meshRenderer.material.color : Color.white;
        }
    }
}