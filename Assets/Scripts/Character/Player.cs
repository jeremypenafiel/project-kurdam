using System;
using System.Collections;
using System.Collections.Generic;
using Items;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Rendering.Universal;

public class Player : MonoBehaviour
{
    [SerializeField] AswangBase playertype;
    [SerializeField] int playerlevel;
    Aswang player;
    [SerializeField] public Inventory inventory;
    [SerializeField] private Light2D suga;

    private void Start()
    {
        player = new Aswang(playertype,playerlevel);
        inventory.OnSugaEquipped += CheckForSuga;
        inventory.ConnectPlayerToController(player);
        suga.gameObject.SetActive(false);
        // inventorySystem.Controller._itemsModel.OnEquippedItemsChanged += OnEquippedItemsChanged;
    }

    private void OnEquippedItemsChanged()
    {
        //player.Base.OnEquippedItemsChanged(inventorySystem.Controller._itemsModel.equippedItems);
    }

    public Aswang GetPlayer()
    {
        return player;
    }
    private void CheckForSuga()
    {
        if (player.EquippedItems[(EquippableItemsBase.ItemType.suga)] != null )
        {
            Debug.Log("Suga equipped!");
            suga.gameObject.SetActive(true);
            suga.intensity = player.EquippedItems[(EquippableItemsBase.ItemType.suga)].Details.lightIntensity;
            suga.color = player.EquippedItems[(EquippableItemsBase.ItemType.suga)].Details.lightTemperature;

            // Do something when suga is equipped
        }
        else
        {
            suga.gameObject.SetActive(false);
            Debug.Log("Suga not equipped.");
            // Do something when suga is not equipped
        }
    }

}
