using System;
using System.Collections;
using System.Collections.Generic;
using Items;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public abstract class ItemsBase: ScriptableObject
{
    [SerializeField] public string itemName;
    [SerializeField] public Sprite icon;
    [SerializeField] public string description;
    [SerializeField] public int maxStack;
    [SerializeField] public bool isConsumable;
    [SerializeField] public bool isMissionItem;
    [SerializeField] public bool isEquipment;
    public SerializableGuid Id = SerializableGuid.NewGuid();

    public abstract Item Create(int quantity);


}


