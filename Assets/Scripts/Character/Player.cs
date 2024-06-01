using System;
using System.Collections;
using System.Collections.Generic;
using Items;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Player : MonoBehaviour
{
    [SerializeField] AswangBase playertype;
    [SerializeField] int playerlevel;
    Aswang player;
    [SerializeField] public Inventory inventory;
    [SerializeField] public TextMeshProUGUI text;

    private void Start()
    {
        player = new Aswang(playertype,playerlevel);
        inventory.ConnectPlayerToController(player);
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

    private void Update()
    {
        SetplayerHealthTExt();
    }

    public void SetplayerHealthTExt()
    {
        text.text = player.HP.ToString();
    }
}
