
using Items;
using UnityEngine;


public class ConsumableItem : Item
{
    public ConsumableItem(ItemsBase details, int quantity) : base(details, quantity)
    {
        Id = SerializableGuid.NewGuid();
        this.details = details;
        this.quantity = quantity;
    }
    
    public void Use(Aswang player)
    {
        ConsumableItemBase consumableItemDetails = (ConsumableItemBase) details;
        foreach (var effect in consumableItemDetails.effects)
        {
            effect.ExecuteEffect(player);
        }
    }
}