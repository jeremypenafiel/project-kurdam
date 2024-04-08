using UnityEngine;

namespace Items
{
    public class ItemController
    {
        readonly ItemsView _itemsView;
        readonly ItemsModel _itemsModel;

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
                icon.RegisterListener(OnItemIconSelected);
            }
            _itemsView.UpdateEquippedItems(_itemsModel.equippedItems);
            _itemsView.UpdateInventoryItems(_itemsModel.inventoryItems);
            
        }
        
        void OnItemIconSelected(int index)
        {
            var item = _itemsModel.inventoryItems[index];
            _itemsModel.Equip(item);
        }

        public class Builder
        {
            readonly ItemsModel _itemsModel = new ItemsModel();

            public Builder WithItems(ItemsBase[] itemData)
            {
                foreach (var data in itemData)
                {
                    _itemsModel.AddItem(new Item(data));
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