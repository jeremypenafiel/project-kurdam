namespace Items
{
    public class ConsumableItem : Item
    {
        public ConsumableItemBase ConsumableItemData;
        public int Amount { get; set; }
        public ConsumableItem(ConsumableItemBase itemData) : base(itemData)
        {
            ConsumableItemData = itemData;
            this.itemData = itemData;
            Amount = 1;
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