using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Melon
{
    public class UIText : GameElement
    {
        public TextMeshProUGUI label;
        public string textValue;

        public void SetText(string inText)
        {
            label.text = inText;
            textValue = inText;
        }

        public override void DoPostShow(object inData = null, ActionResult inActionResult = null)
        {
            base.DoPostShow(inData);
            
            if(inData is string str)
            {
                SetText(str);
            }
        }
    }
}
