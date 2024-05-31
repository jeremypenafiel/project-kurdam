using System;
using System.Collections;
using System.Collections.Generic;
using Items;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemsBase: ScriptableObject
{
    [SerializeField] public string itemName;
    [SerializeField] public Sprite icon;
    [SerializeField] public string description;
    [SerializeField] public int maxStack;
    public SerializableGuid Id = SerializableGuid.NewGuid();

    public Item Create(int quantity)
    {
        return new Item(this, quantity);
    }
}


