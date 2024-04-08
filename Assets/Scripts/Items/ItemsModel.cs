using System;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    public class ItemsModel
    {
        public readonly List<Item> inventoryItems = new();
        public readonly Dictionary<ItemsBase.ItemType,Item> equippedItems = new()
        {
            { ItemsBase.ItemType.armasIsa, null},
            { ItemsBase.ItemType.armasDuha, null},
            { ItemsBase.ItemType.ulo, null},
            { ItemsBase.ItemType.antingAntingIsa, null},
            { ItemsBase.ItemType.antingAntingDuha, null},
            { ItemsBase.ItemType.singSingIsa, null},
            { ItemsBase.ItemType.singSingDuha, null},
            { ItemsBase.ItemType.lawas, null},
            { ItemsBase.ItemType.paaIsa, null},
            { ItemsBase.ItemType.paaDuha, null},
            { ItemsBase.ItemType.tiil, null},
            { ItemsBase.ItemType.kamot, null},
            { ItemsBase.ItemType.gamit, null}
        };
        public event Action OnInventoryChanged;
        public event Action OnEquippedItemsChanged;


        public void AddItem(Item item)
        {
            inventoryItems.Add(item);
            Debug.Log("Item added in model");
            OnInventoryChanged?.Invoke();
        }

        public void RemoveItem(Item item)
        {
            inventoryItems.Remove(item);
            OnInventoryChanged?.Invoke();
        }
        
        public void Equip(Item item)
        {
            equippedItems[item.itemData.type] = item;
            inventoryItems.Remove(item);
            OnEquippedItemsChanged?.Invoke();
        }
        
        public void Unequip(Item item)
        {
            equippedItems[item.itemData.type] = null;
            inventoryItems.Add(item);
            OnEquippedItemsChanged();
        }
    }
    
    public class Item
    {
        public ItemsBase itemData;

        public Item(ItemsBase itemData)
        {
            this.itemData = itemData;
        }
    }
        
}