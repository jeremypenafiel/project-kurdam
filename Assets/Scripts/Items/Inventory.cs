using System.Collections;
using System.Collections.Generic;
using Items;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private InventoryView view;
    [SerializeField]  int capacity = 20;
    [SerializeField] private List<ItemsBase> startingItems = new List<ItemsBase>();
    [SerializeField] private List<EquippableItemsBase> startingEquippedItems = new List<EquippableItemsBase>();

    InventoryController controller;


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
    }
}
