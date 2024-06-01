using Items;
using UnityEngine;

[CreateAssetMenu(fileName = "New Consumable Item", menuName = "Items/Consumable Item")]
public class ConsumableItemBase : ItemsBase
{
    public ConsumableItemEffect[] effects;

    public override Item Create(int quantity)
    {
        return new ConsumableItem(this, quantity);
    }
}