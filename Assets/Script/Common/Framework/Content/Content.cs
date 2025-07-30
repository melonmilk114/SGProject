using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Melon
{
    public class Content: GameElement, IContent
    {
        public virtual void InitContent()
        {
            
        }

        public virtual Enum GetContentType()
        {
            return default;
        }
        
        public virtual void DoContentStart(object inData)
        {
            
        }

        
    }
}