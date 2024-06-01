
    using Items;

    public class EquippableItem: Item
    {
        public EquippableItemsBase Details => (EquippableItemsBase)this.details;

        public EquippableItem(ItemsBase details, int quantity) : base(details, quantity)
        {
            Id = SerializableGuid.NewGuid();
            this.details = details;
            this.quantity = quantity;
        }
    }
