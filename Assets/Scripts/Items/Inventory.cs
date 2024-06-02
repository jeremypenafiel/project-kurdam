using System.Collections;
using System.Collections.Generic;
using Items;
using UnityEngine;
using System;

public class Inventory : MonoBehaviour
{
    [SerializeField] private InventoryView view;
    [SerializeField]  int capacity = 20;
    [SerializeField] private List<ItemsBase> startingItems = new List<ItemsBase>();
    [SerializeField] private List<EquippableItemsBase> startingEquippedItems = new List<EquippableItemsBase>();

    InventoryController controller;
    

    public event Action OnExitPressed
    {
        add => view.OnExitPressed += value;
        remove => view.OnExitPressed -= value;
    }

    public event Action OnSugaEquipped
    {
        add => controller.OnSugaEquipped += value;
        remove => controller.OnSugaEquipped -= value;
    }

    void Awake()
    {
        controller = new InventoryController.Builder(view)
            .WithStartingItems(startingItems)
            .WithStartingEquipment(startingEquippedItems)
            .WithCapacity(capacity)
            .Build();

    }

    public void ConnectPlayerToController(Aswang player)
    {
        controller.player = player;
        controller.UpdatePlayerEquipment();
    }

    public void ActivateView(bool isActive)
    {
        view.Activate(isActive);
    }
    
    public bool ContainsItem(ItemsBase item)
    {
        return controller.ContainsItem(item);
    }
    
    public void AddItem(ItemsBase item)
    {
        controller.AddItem(item);
    }

    public bool IsInventoryFull()
    {
        return controller.IsInventoryFull();
    }
}
