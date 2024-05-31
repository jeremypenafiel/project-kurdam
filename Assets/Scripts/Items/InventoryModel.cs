using System;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    public class InventoryModel
    {

        public ObservableArray<Item> Items { get; set; }
        
        public event Action<Item[]> OnModelChanged
        {
            add => Items.AnyValueChanged += value;
            remove => Items.AnyValueChanged -= value;
            
        }

        public InventoryModel(IEnumerable<ItemsBase> itemDetails, int capacity)
        {
            Items = new ObservableArray<Item>(capacity);
            foreach (var itemDetail in itemDetails)
            {
                Items.TryAdd(itemDetail.Create(1));
            }
        }

        public Item Get(int index) => Items[index];
        public void Clear() => Items.Clear();
        public bool Add(Item item) => Items.TryAdd(item);
        
        public bool Remove(Item item) => Items.TryRemove(item);

        public void Swap(int source, int target) => Items.Swap(source, target);

        public int Combine(int source, int target)
        {
            var total = Items[source].quantity + Items[target].quantity;
            Items[source].quantity = total;
            Remove(Items[source]);
            return total;

        }
        
        

        // public readonly List<Item> inventoryItems = new();
        // public readonly Dictionary<EquippableItemsBase.ItemType,EquippableItem> equippedItems = new()
        // {
        //     { EquippableItemsBase.ItemType.armasIsa, null},
        //     { EquippableItemsBase.ItemType.armasDuha, null},
        //     { EquippableItemsBase.ItemType.ulo, null},
        //     { EquippableItemsBase.ItemType.antingAntingIsa, null},
        //     { EquippableItemsBase.ItemType.antingAntingDuha, null},
        //     { EquippableItemsBase.ItemType.singSingIsa, null},
        //     { EquippableItemsBase.ItemType.singSingDuha, null},
        //     { EquippableItemsBase.ItemType.lawas, null},
        //     { EquippableItemsBase.ItemType.paaIsa, null},
        //     { EquippableItemsBase.ItemType.paaDuha, null},
        //     { EquippableItemsBase.ItemType.tiil, null},
        //     { EquippableItemsBase.ItemType.kamot, null},
        //     { EquippableItemsBase.ItemType.gamit, null}
        // };
        public event Action OnInventoryChanged;
        public event Action OnEquippedItemsChanged;


        // public void AddItem(Item newItem)
        // {
        //     // if item is consumable and there is already an item of the same type, increment amount
        //     if (newItem.itemData is ConsumableItemBase )
        //     {
        //         var item = inventoryItems.Find(item => (item.itemData.name == newItem.itemData.name));
        //         if (item != null)
        //         {
        //             var consumableItem = (ConsumableItem) item;
        //             consumableItem.Amount++;
        //             Debug.Log(consumableItem.Amount);
        //             OnInventoryChanged?.Invoke();
        //             return;
        //         }
        //     }
            
        //     // else add to inventory
        //     inventoryItems.Add(newItem);
        //     Debug.Log("Item added in model");
        //     OnInventoryChanged?.Invoke();
        // }

        // public void RemoveItem(Item item)
        // {
        //     inventoryItems.Remove(item);
        //     OnInventoryChanged?.Invoke();
        // }
        //
        // public void ConsumeItem(ConsumableItem consumableItem)
        // {
        //     inventoryItems.Remove(consumableItem);
        //     OnInventoryChanged?.Invoke();
        // }
        
        // public void Equip(EquippableItem item)
        // {
        //     if(equippedItems[item.EquipableItemData.type] != null)
        //     {
        //         Unequip(equippedItems[item.EquipableItemData.type]);
        //     }
        //     equippedItems[item.EquipableItemData.type] = item;
        //     inventoryItems.Remove(item);
        //     OnEquippedItemsChanged?.Invoke();
        //     OnInventoryChanged?.Invoke();
        // }
        
        // public void Unequip(EquippableItem item)
        // {
        //     equippedItems[item.EquipableItemData.type] = null;
        //     inventoryItems.Add(item);
        //     OnEquippedItemsChanged?.Invoke();
        //     OnInventoryChanged?.Invoke();
        // }
        
        // public bool Contains(ItemsBase requiredItem)
        // {
        //     foreach (var item in inventoryItems)
        //     {
        //         if(Equals(item.itemData, requiredItem))
        //         {
        //             return true;
        //         }
        //     }
        //     return false;
        // }
        //
        // public void RemoveItem(ItemsBase requiredItem)
        // {
        //     foreach (var item in inventoryItems)
        //     {
        //         if(Equals(item.itemData, requiredItem))
        //         {
        //             inventoryItems.Remove(item);
        //             OnInventoryChanged?.Invoke();
        //             return;
        //         }
        //     }
        // }
        
        public void AddItem(ItemsBase rewardItem)
        {
            if(rewardItem is EquippableItemsBase)
            {
                //AddItem(new EquippableItem((EquippableItemsBase)rewardItem));
                return;
            }
            
            if (rewardItem is ConsumableItemBase)
            {
                //AddItem(new ConsumableItem((ConsumableItemBase)rewardItem));
            }
            
        }
    
    }
    
        
    
    // public abstract class Item
    // {
    //     public ItemsBase itemData;
    //
    //     public Item(ItemsBase itemData)
    //     {
    //         this.itemData = itemData;
    //     }
    //     
    // }
        
}