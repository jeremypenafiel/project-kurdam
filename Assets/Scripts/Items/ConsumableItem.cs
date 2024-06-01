
public class ConsumableItem : Item
{
    public ConsumableItem(ItemsBase details, int quantity) : base(details, quantity)
    {
        Id = SerializableGuid.NewGuid();
        this.details = details;
        this.quantity = quantity;
    }
}