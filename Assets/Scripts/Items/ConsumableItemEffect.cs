using UnityEngine;

namespace Items
{
    public interface IConsumableItemEffect
    {
        public void ExecuteEffect(ConsumableItemBase itemData, Player player);
    }
}