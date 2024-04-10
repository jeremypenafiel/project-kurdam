using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemsBase: ScriptableObject
{
    [SerializeField] public string itemName;
    [SerializeField] public Sprite icon;
    [SerializeField] public string description;
}


