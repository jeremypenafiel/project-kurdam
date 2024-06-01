using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Items
{
    public class BindableProperty<T>
    {
        readonly Func<T> getter;

        BindableProperty(Func<T> getter)
        {
            this.getter = getter;
        }

        [CreateProperty] public T Value => getter();

        public static BindableProperty<T> Bind(Func<T> getter) => new BindableProperty<T>(getter);
    }
    public class ViewModel
    {
        public readonly int Capacity;
        public readonly BindableProperty<Item> item;
        
        public ViewModel(InventoryModel model, int capacity)
        {
            Capacity = capacity;
            item = BindableProperty<Item>.Bind(() => model.currentItem);
        }
    }
    public class InventoryController
    {
        readonly InventoryView view;
        readonly InventoryModel model;
        readonly int capacity;
        public Aswang player { get; set; }

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
            view.OnInventoryItemSelectionChanged += HandleOnInventoryItemSelectionChanged;
            view.OnEquipmentItemSelectionChanged += HandleOnEquipmentItemSelectionChanged;
            view.OnInventoryActionSelected += HandleOnInventoryActionSelected;
            view.OnEquipmentActionSelected += HandleOnEquipmentActionSelected;
            view.CheckIfMissionItem += HandleCheckIfMissionItem;
            model.OnModelChanged += HandleModelChanged;
            model.OnEquipmentChanged += HandleModelChanged;
            
            yield return view.InitializeView(new ViewModel(model, capacity));
            

            RefreshView();

        }

        private bool HandleCheckIfMissionItem(int itemIndex)
        {
            var item = model.GetFromInventory(itemIndex);
            return item.details.isMissionItem;
        }

        private void HandleOnEquipmentActionSelected(int action, int itemIndex)
        {
            var item = model.GetFromEquipment(itemIndex);
            if (action == 0)
            {
                
            }
            else
            {
                model.RemoveFromEquipment(item);
            }
        }

        private void HandleOnEquipmentItemSelectionChanged(int index)
        {
            Debug.Log(index);
            var item = model.GetFromEquipment(index);
            model.currentItem = item;
            view.SetItemDescriptionBox(item);
        }

        private void HandleOnInventoryActionSelected(int action, int itemIndex) // action = 0 is use/equip, action = 1 is discard
        {
            
            if (action == 0)
            {
                var item = model.GetFromInventory(itemIndex);
                item.Use(player);
                model.RemoveFromInventory(item);
            }
            else
            {
                var item = model.GetFromInventory(itemIndex);
                model.RemoveFromInventory(item);
            }
        }

        void HandleOnInventoryItemSelectionChanged(int index)
        {
            Debug.Log("nagrun");
            var item = model.GetFromInventory(index);
            model.currentItem = item;
            view.SetItemDescriptionBox(item);
        }

        void HandleModelChanged(IList<Item> items)
        {
            model.currentItem = null;
            view.SetItemDescriptionBox(model.currentItem);
            RefreshView();
            
        }
            


        void RefreshView()
        {
            for (int i = 0; i < capacity; i++)
            {
                var item = model.GetFromInventory(i);
                if (item == null)
                {
                    view.InventorySlots[i].Set(SerializableGuid.Empty, null);
                }
                else
                {
                    view.InventorySlots[i].Set(item.Id, item.details.icon, item.quantity);
                }
            }
            
            for(int i = 0; i < 6; i++)
            {
                var item = model.GetFromEquipment(i);
                if (item == null)
                {
                    view.EquipmentSlots[i].Set(SerializableGuid.Empty, null);
                }
                else
                {
                    view.EquipmentSlots[i].Set(item.Id, item.details.icon, item.quantity);
                }
            }
        }
        
       

        #region Builder

        public class Builder
        {
            private InventoryView view;
            IEnumerable<ItemsBase> itemDetails;
            IEnumerable<ItemsBase> equipmentDetails;
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
            
            public Builder WithStartingEquipment(IEnumerable <ItemsBase> equipmentDetails)
            {
                this.equipmentDetails = equipmentDetails;
                return this;
            }
            
            public Builder WithCapacity(int capacity)
            {
                this.capacity = capacity;
                return this;
            }

            public InventoryController Build()
            {
                InventoryModel model;
                if (itemDetails == null && equipmentDetails == null)
                {
                    model = new InventoryModel(Array.Empty<ItemsBase>(), Array.Empty<ItemsBase>(), capacity);
                }else if (itemDetails != null && equipmentDetails != null)
                {
                    model = new InventoryModel(itemDetails, equipmentDetails, capacity);
                }else if (itemDetails != null)
                {
                    model = new InventoryModel(itemDetails, Array.Empty<ItemsBase>(), capacity);
                }
                else
                {
                    model = new InventoryModel(Array.Empty<ItemsBase>(), equipmentDetails, capacity);
                }

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