using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Melon
{
    public class UIImage : GameElement
    {
        public Image image;

        public void SetImage(Sprite inSprite)
        {
            if (inSprite == null)
                return;
            
            image.sprite = inSprite;
        }

        public void SetImage(string inFilePath)
        {
            Sprite sprite = Resources.Load<Sprite>(inFilePath);
            SetImage(sprite);
        }

        public override void DoPostShow(object inData = null, ActionResult inActionResult = null)
        {
            base.DoPostShow(inData, inActionResult);
            
            if(inData is Sprite sprite)
            {
                SetImage(sprite);
            }
            else if(inData is string filePath)
            {
                SetImage(filePath);
            }
        }
    }
}
