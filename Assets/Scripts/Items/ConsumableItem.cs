namespace Items
{
    public class ConsumableItem : Item
    {
        public ConsumableItemBase ConsumableItemData;
        public ConsumableItem(ConsumableItemBase itemData) : base(itemData)
        {
            ConsumableItemData = itemData;
            this.itemData = itemData;
        }

        public void Consume(Player player)
        {
            foreach (var effect in ConsumableItemData.itemEffects)
            {
                effect.ExecuteEffect(ConsumableItemData, player);
            }
        }
    }
}