using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    public class ItemController
    {
        readonly ItemsView _itemsView;
        public readonly ItemsModel _itemsModel;

        ItemController(ItemsView itemsView, ItemsModel itemsModel)
        {
            _itemsView = itemsView;
            _itemsModel = itemsModel;

            ConnectModel();
            ConnectView();
        }

        public void Update(float deltaTime)
        {
            
        }
        
        public void ConnectModel()
        {
            _itemsModel.OnInventoryChanged += UpdateInventoryItems;
            _itemsModel.OnEquippedItemsChanged += UpdateEquippedItems;
            
        }
        
        void UpdateInventoryItems()
        {
            _itemsView.UpdateInventoryItems(_itemsModel.inventoryItems);
        }
        
        void UpdateEquippedItems()
        {
            _itemsView.UpdateEquippedItems(_itemsModel.equippedItems);
        }
        
        public void ConnectView()
        {
            foreach (var icon in _itemsView.inventoryIcons)
            {
                icon.RegisterSelectedListener(OnItemIconSelected);
                icon.RegisterActionSelectedListener(OnItemActionSelected);
            }
            _itemsView.RegisterSelectionListener(onSelectionChanged);
            _itemsView.UpdateEquippedItems(_itemsModel.equippedItems);
            _itemsView.UpdateInventoryItems(_itemsModel.inventoryItems);
            
        }

        ItemsBase onSelectionChanged(int index)
        {
            var itemData = index >= _itemsModel.inventoryItems.Count || index < 0  ? null : _itemsModel.inventoryItems[index].itemData;
            return itemData;
        }
        
        
        
        void OnItemIconSelected(int index)
        {
            var item = _itemsModel.inventoryItems[index];
            _itemsView.EnableDialogBox(item is EquippableItem);
            
        }

        void OnItemActionSelected(int index, int action)
        {
            Debug.Log("garun");
            var item = _itemsModel.inventoryItems[index];

            if (action == 1)
            {
                _itemsModel.RemoveItem(item);
                return;
            }

            if (item is EquippableItem equippableItem)
            {
                _itemsModel.Equip(equippableItem);
            }
            else
            {
                _itemsModel.ConsumeItem((ConsumableItem)item);
            }
            
        }

        public class Builder
        {
            readonly ItemsModel _itemsModel = new ItemsModel();

            public Builder WithItems(ItemsBase[] itemData)
            {
                foreach (var data in itemData)
                {
                    switch (data)
                    {
                        case ConsumableItemBase:
                            _itemsModel.AddItem(new ConsumableItem((ConsumableItemBase) data));
                            break;
                        case EquippableItemsBase:
                            _itemsModel.AddItem(new EquippableItem((EquippableItemsBase) data));
                            break;
                    }
                }

                return this;
            }

            public ItemController Build(ItemsView view)
            {
                return view == null ? null : new ItemController(view, _itemsModel);
            }
        }
 
    }
}