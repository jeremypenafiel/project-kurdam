using System;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    public class ItemsModel
    {
        public readonly List<Item> inventoryItems = new();
        public readonly Dictionary<EquippableItemsBase.ItemType,EquippableItem> equippedItems = new()
        {
            { EquippableItemsBase.ItemType.armasIsa, null},
            { EquippableItemsBase.ItemType.armasDuha, null},
            { EquippableItemsBase.ItemType.ulo, null},
            { EquippableItemsBase.ItemType.antingAntingIsa, null},
            { EquippableItemsBase.ItemType.antingAntingDuha, null},
            { EquippableItemsBase.ItemType.singSingIsa, null},
            { EquippableItemsBase.ItemType.singSingDuha, null},
            { EquippableItemsBase.ItemType.lawas, null},
            { EquippableItemsBase.ItemType.paaIsa, null},
            { EquippableItemsBase.ItemType.paaDuha, null},
            { EquippableItemsBase.ItemType.tiil, null},
            { EquippableItemsBase.ItemType.kamot, null},
            { EquippableItemsBase.ItemType.gamit, null}
        };
        public event Action OnInventoryChanged;
        public event Action OnEquippedItemsChanged;


        public void AddItem(Item item)
        {
            if (item.itemData is ConsumableItemBase)
            {
                /*item.itemData.*/
            }
            inventoryItems.Add(item);
            Debug.Log("Item added in model");
            OnInventoryChanged?.Invoke();
        }

        public void RemoveItem(Item item)
        {
            inventoryItems.Remove(item);
            OnInventoryChanged?.Invoke();
        }

        public void ConsumeItem()
        {
            
        }
        
        public void Equip(EquippableItem item)
        {
            if(equippedItems[item.EquipableItemData.type] != null)
            {
                Unequip(equippedItems[item.EquipableItemData.type]);
            }
            equippedItems[item.EquipableItemData.type] = item;
            inventoryItems.Remove(item);
            OnEquippedItemsChanged?.Invoke();
            OnInventoryChanged?.Invoke();
        }
        
        public void Unequip(EquippableItem item)
        {
            equippedItems[item.EquipableItemData.type] = null;
            inventoryItems.Add(item);
            OnEquippedItemsChanged?.Invoke();
            OnInventoryChanged?.Invoke();
        }
    }
    
    public abstract class Item
    {
        public ItemsBase itemData;

        public Item(ItemsBase itemData)
        {
            this.itemData = itemData;
        }
        
    }
        
}