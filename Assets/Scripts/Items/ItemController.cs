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
            Debug.Log("Model connected");
            _itemsModel.OnInventoryChanged += UpdateInventoryItems;
            _itemsModel.OnEquippedItemsChanged += UpdateEquippedItems;
            
        }
        
        void UpdateInventoryItems()
        {
            Debug.Log("inventory changed");
            _itemsView.UpdateInventoryItems(_itemsModel.inventoryItems);
        }
        
        void UpdateEquippedItems()
        {
            _itemsView.UpdateEquippedItems(_itemsModel.equippedItems);
        }
        
        public void ConnectView()
        {
            /*foreach (var icon in _itemsView.inventoryIcons)
            {
                icon.RegisterListener(OnItemIconSelected);
            }*/
            Debug.Log("View connected");
            _itemsView.UpdateEquippedItems(_itemsModel.equippedItems);
            _itemsView.UpdateInventoryItems(_itemsModel.inventoryItems);
            
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