using System;
using System.Collections;
using System.Collections.Generic;
using Items;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class StorageView : MonoBehaviour
{
    public Slot[] InventorySlots;
    public Slot[] EquipmentSlots;

    [SerializeField] protected UIDocument document;
    [SerializeField] protected StyleSheet styleSheet;

    protected static VisualElement ghostIcon;

    protected VisualElement root;
    protected VisualElement container;

  

    public abstract IEnumerator InitializeView(int capacity);
    

}

