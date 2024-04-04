using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InventorySystemState { Navigation, ActionSelection}
public class InventorySystem : MonoBehaviour
{
    [SerializeField] List<GameObject> inventory;

    InventorySystemState state;
    public static InventorySystem i;
    int currentNavigation;
    int currentActionSelection;


    private void Awake()
    {
        i = this;
    }

    public void StartInventorySystem()
    {
        currentNavigation = 11;
        state = InventorySystemState.Navigation;
        HandleUpdate();
    }

    public void HandleUpdate()
    {
        if (state== InventorySystemState.Navigation)
        {
            HandleActionSelection();
        }
        if (state== InventorySystemState.ActionSelection)
        {
           
        }
    }

    public void HandleActionSelection()
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
        UpdateInventoryNavigation(currentNavigation);

    }

    public void UpdateInventoryNavigation(int currentIndexHighlight)
    {
        for (int i = 0; i < inventory.Count; ++i)
        {
            if (i == currentIndexHighlight)
            {
                
                inventory[i].gameObject.SetActive(true);
            }
            else
            {
                inventory[i].gameObject.SetActive(false);
            }
        }
        

    }
}
