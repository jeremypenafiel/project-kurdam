using UnityEngine;

namespace Items
{
    [CreateAssetMenu(fileName = "New Health Modifier", menuName = "Inventory/Items/Health Modifier")]
    public class HealthModifier: ConsumableItemEffect
    {
        public int amount;
        public override void ExecuteEffect(Aswang player)
        {
            player.HP += amount;
        }
    }
}