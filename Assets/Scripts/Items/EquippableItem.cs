namespace Items
{
    public class EquippableItem: Item
    {
        public EquippableItemsBase EquipableItemData;
        public EquippableItem(EquippableItemsBase itemData) : base(itemData)
        {
            EquipableItemData = itemData;
            this.itemData = itemData;
        }
    }
}