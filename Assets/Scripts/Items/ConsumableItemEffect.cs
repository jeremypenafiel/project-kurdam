using System;
using UnityEngine;

namespace Items
{
    public abstract class ConsumableItemEffect : ScriptableObject
    {
        public abstract void ExecuteEffect(Aswang player);
    }
    
}