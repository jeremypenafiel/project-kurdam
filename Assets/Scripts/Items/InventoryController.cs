using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Items
{
    public class InventoryController
    {
        readonly InventoryView view;
        readonly InventoryModel model;
        readonly int capacity;

        InventoryController(InventoryView view, InventoryModel model, int capacity)
        {
            Debug.Assert(view != null, "View is null");
            Debug.Assert(model != null, "Model is null");
            Debug.Assert(capacity > 0, "Capacity is less than 1");

            this.view = view;
            this.model = model;
            this.capacity = capacity;

            view.StartCoroutine(Initialize());
        }

        IEnumerator Initialize()
        {
            yield return view.InitializeView(capacity);

            model.OnModelChanged += HandleModelChanged;

            RefreshView();

        }

        void HandleModelChanged(IList<Item> items) => RefreshView();


        void RefreshView()
        {
            for (int i = 0; i < capacity; i++)
            {
                var item = model.Get(i);
                if (item == null)
                {
                    view.Slots[i].Set(SerializableGuid.Empty, null);
                }
                else
                {
                    view.Slots[i].Set(item.Id, item.details.icon, item.quantity);
                }
            }
        }

        #region Builder

        public class Builder
        {
            private InventoryView view;
            IEnumerable<ItemsBase> itemDetails;
            int capacity = 20;

            public Builder(InventoryView view)
            {
                this.view = view;
            }

            public Builder WithStartingItems(IEnumerable<ItemsBase> itemDetails)
            {
                this.itemDetails = itemDetails;
                return this;
            }
            
            public Builder WithCapacity(int capacity)
            {
                this.capacity = capacity;
                return this;
            }

            public InventoryController Build()
            {
                InventoryModel model = itemDetails != null
                    ? new InventoryModel(itemDetails, capacity)
                    : new InventoryModel(Array.Empty<ItemsBase>(), capacity);

                return new InventoryController(view, model, capacity);
            }



        }

        #endregion Builder

        // public class ItemController
        // {
        //     // readonly ItemsView _itemsView;
        //     // public readonly ItemsModel _itemsModel;
        //     //
        //     // ItemController(ItemsView itemsView, ItemsModel itemsModel)
        //     // {
        //     //     _itemsView = itemsView;
        //     //     _itemsModel = itemsModel;
        //     //
        //     //     ConnectModel();
        //     //     ConnectView();
        //     // }
        //     //
        //     // public void Update(float deltaTime)
        //     // {
        //     //     
        //     // }
        //     //
        //     // public void ConnectModel()
        //     // {
        //     //     _itemsModel.OnInventoryChanged += UpdateInventoryItems;
        //     //     _itemsModel.OnEquippedItemsChanged += UpdateEquippedItems;
        //     //     
        //     // }
        //     //
        //     // void UpdateInventoryItems()
        //     // {
        //     //     _itemsView.UpdateInventoryItems(_itemsModel.inventoryItems);
        //     // }
        //     //
        //     // void UpdateEquippedItems()
        //     // {
        //     //     _itemsView.UpdateEquippedItems(_itemsModel.equippedItems);
        //     // }
        //     //
        //     // public void ConnectView()
        //     // {
        //     //     foreach (var icon in _itemsView.inventoryIcons)
        //     //     {
        //     //         icon.RegisterSelectedListener(OnItemIconSelected);
        //     //         icon.RegisterActionSelectedListener(OnItemActionSelected);
        //     //     }
        //     //     _itemsView.RegisterSelectionListener(onSelectionChanged);
        //     //     _itemsView.UpdateEquippedItems(_itemsModel.equippedItems);
        //     //     _itemsView.UpdateInventoryItems(_itemsModel.inventoryItems);
        //     //     
        //     // }
        //     //
        //     // Item onSelectionChanged(int index)
        //     // {
        //     //     var itemData = index >= _itemsModel.inventoryItems.Count || index < 0  ? null : _itemsModel.inventoryItems[index];
        //     //     return itemData;
        //     // }
        //     //
        //     //
        //     //
        //     // void OnItemIconSelected(int index)
        //     // {
        //     //     var item = _itemsModel.inventoryItems[index];
        //     //     _itemsView.EnableDialogBox(item is EquippableItem);
        //     //     
        //     // }
        //
        //     // void OnItemActionSelected(int index, int action)
        //     // {
        //     //     Debug.Log("garun");
        //     //     var item = _itemsModel.inventoryItems[index];
        //     //
        //     //     if (action == 1)
        //     //     {
        //     //         _itemsModel.RemoveItem(item);
        //     //         return;
        //     //     }
        //     //
        //     //     if (item is EquippableItem equippableItem)
        //     //     {
        //     //         _itemsModel.Equip(equippableItem);
        //     //     }
        //     //     else
        //     //     {
        //     //         _itemsModel.ConsumeItem((ConsumableItem)item);
        //     //     }
        //     //     
        //     // }
        //
        //     public class Builder
        //     {
        //         readonly ItemsModel _itemsModel = new ItemsModel();
        //
        //         public Builder WithItems(ItemsBase[] itemData)
        //         {
        //             // foreach (var data in itemData)
        //             // {
        //             //     switch (data)
        //             //     {
        //             //         case ConsumableItemBase:
        //             //             _itemsModel.AddItem(new ConsumableItem((ConsumableItemBase) data));
        //             //             break;
        //             //         case EquippableItemsBase:
        //             //             _itemsModel.AddItem(new EquippableItem((EquippableItemsBase) data));
        //             //             break;
        //             //     }
        //             // }
        //             //
        //             // return this;
        //         }
        //
        //     //     public ItemController Build(ItemsView view)
        //     //     {
        //     //         return view == null ? null : new ItemController(view, _itemsModel);
        //     //     }
        //     // }
        //
        // }
    }
}