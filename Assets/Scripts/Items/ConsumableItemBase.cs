using System;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    [CreateAssetMenu(fileName = "Item", menuName = "Items/ConsumableItem")]
    public class ConsumableItemBase : ItemsBase
    {
        public readonly bool IsStackable = true;
        public int currentStackNumber = 0;
        public List<ConsumableItemEffect> itemEffects;
  
    }
}