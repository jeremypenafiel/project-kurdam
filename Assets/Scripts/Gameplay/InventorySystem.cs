using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public enum InventorySystemState { Navigation, ActionSelection}
public class InventorySystem : MonoBehaviour
{
    [SerializeField] List<GameObject> inventoryHighlight;
    [SerializeField] List<GameObject> inventoryIcon;
    [SerializeField] List<ItemsBase?> inventoryItems;

    InventorySystemState state;
    public static InventorySystem i;
    int currentNavigation;
    int currentActionSelection;

    public event Action SelectCloseInventory;
    public Aswang iplayer;

    private void Awake()
    {
        i = this;
    }

    public void StartInventorySystem(Aswang player)
    {
        iplayer = player;

        //refactor thrugh creating method and loop theough
        inventoryItems[0] = player.Base.EquippedItems.Ulo;
        inventoryItems[1] = player.Base.EquippedItems.AntingAntingIsa;
        inventoryItems[2] = player.Base.EquippedItems.Lawas;
        inventoryItems[3] = player.Base.EquippedItems.AntingAntingDuha;
        inventoryItems[4] = player.Base.EquippedItems.ArmasIsa;
        inventoryItems[5] = player.Base.EquippedItems.SingSingIsa;
        inventoryItems[6] = player.Base.EquippedItems.Tiil;
        inventoryItems[7] = player.Base.EquippedItems.SingSingDuha;
        inventoryItems[8] = player.Base.EquippedItems.ArmasDuha;
        inventoryItems[9] = player.Base.EquippedItems.PaaIsa;
        inventoryItems[10] = player.Base.EquippedItems.PaaDuha;

        for (int i = 0; i < player.Base.EquippedItems.Gamit.Count; i++)
        {
            inventoryItems[i + 11] = player.Base.EquippedItems.Gamit[i];
        }




        for (int i = 0; i < inventoryItems.Count; i++)
        {
            if (inventoryItems[i] != null) 
            {
                inventoryIcon[i].SetActive(true);
                inventoryIcon[i].GetComponent<SpriteRenderer>().sprite = inventoryItems[i].Icon;
            }
        }


        currentNavigation = 11;
        state = InventorySystemState.Navigation;
        HandleUpdate();
    }

    public void HandleUpdate()
    {
        if (state== InventorySystemState.Navigation)
        {
            HandleNavigationSelection();
        }
        if (state== InventorySystemState.ActionSelection)
        {
            HandleActionSelection();
        }
    }

    void HandleNavigationSelection()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (currentNavigation < 34)
            {
                ++currentNavigation;
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currentNavigation > 0)
            {
                --currentNavigation;
            }
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            UpdatePlayerBaseInventory();
            SelectCloseInventory();
        }
        if (Input.GetKeyDown(KeyCode.Z) && (inventoryItems[currentNavigation] != null))
        {
            ActionSelection();
        }
        UpdateInventoryNavigation(currentNavigation);

    }

    void HandleActionSelection()
    {
        
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (currentActionSelection < 1)
            {
                ++currentActionSelection;
                Debug.Log(currentActionSelection);
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (currentActionSelection > 0)
            {
                --currentActionSelection;
                Debug.Log(currentActionSelection);
            }
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if ((currentNavigation > 10) && (currentActionSelection == 0))
            {

            
            Equip(currentNavigation, inventoryItems[currentNavigation]);
            Debug.Log("equipped");
            }
            else if (currentActionSelection == 1)
            {
                Discard(currentNavigation);
                Debug.Log("discarded");
            }
        }
        
        if (Input.GetKeyDown(KeyCode.X))
        {
            Navigation();
        }
        UpdateActionSelection();
    }
    void UpdateInventoryNavigation(int currentIndexHighlight)
    {
        for (int i = 0; i < inventoryHighlight.Count; ++i)
        {
            if (i == currentIndexHighlight)
            {          
                inventoryHighlight[i].SetActive(true);
            }
            else
            {
                inventoryHighlight[i].SetActive(false);
            }
        }
        

    }

    void Equip(int slot, ItemsBase item)
    {
        if (item.Type == ItemsBase.ItemType.armasIsa)
        {
            inventoryItems[slot] = inventoryItems[4];
            inventoryItems[4] = item;
            inventoryIcon[4].GetComponent<SpriteRenderer>().sprite = inventoryItems[4].Icon;
            inventoryIcon[slot].GetComponent<SpriteRenderer>().sprite = inventoryItems[slot].Icon;


        }
    }

    public void Unequip()
    {

    }

    public void Discard(int slot)
    {
        inventoryItems[slot] = null;
        inventoryIcon[slot].SetActive(false);

    }
    public void UpdatePlayerBaseInventory ()
    {
        iplayer.Base.EquippedItems.Ulo = inventoryItems[0];
        iplayer.Base.EquippedItems.AntingAntingIsa = inventoryItems[1];
        iplayer.Base.EquippedItems.Lawas = inventoryItems[2];
        iplayer.Base.EquippedItems.AntingAntingDuha = inventoryItems[3];
        iplayer.Base.EquippedItems.ArmasIsa = inventoryItems[4];
        iplayer.Base.EquippedItems.SingSingIsa = inventoryItems[5];
        iplayer.Base.EquippedItems.Tiil = inventoryItems[6];
        iplayer.Base.EquippedItems.SingSingDuha = inventoryItems[7];
        iplayer.Base.EquippedItems.ArmasDuha = inventoryItems[8];
        iplayer.Base.EquippedItems.PaaIsa = inventoryItems[9];
        iplayer.Base.EquippedItems.PaaDuha = inventoryItems[10];

        for (int i = 0; i < 24; i++)
        {
            iplayer.Base.EquippedItems.Gamit[i]= inventoryItems[i + 11];
             
        }

    }

    public void ActionSelection()
    {
        state = InventorySystemState.ActionSelection;
        Debug.Log("Action Selection");
    }

    public void Navigation()
    {
        state = InventorySystemState.Navigation;
        Debug.Log("Navigation");
    }

    public void UpdateActionSelection()
    {

    }

}
