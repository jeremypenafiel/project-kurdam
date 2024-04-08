using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    public class ItemsView : MonoBehaviour
    {
        [SerializeField] List<GameObject> inventoryHighlight;
        [SerializeField] public ItemIcon[] equippedIcons;
        [SerializeField] public ItemIcon[] inventoryIcons;
        [SerializeField] GameObject itemDescription;
        private int equippedNumberOffset = 11;
        private int currentNavigation;
        private int previousNavigation;

        [SerializeField] public Dictionary<ItemsBase.ItemType, ItemIcon> equippedIconsDictionary = new()
        {
            
            { ItemsBase.ItemType.ulo, null },
            { ItemsBase.ItemType.antingAntingIsa, null },
            { ItemsBase.ItemType.lawas, null },
            { ItemsBase.ItemType.antingAntingDuha, null },
            { ItemsBase.ItemType.armasIsa, null },
            { ItemsBase.ItemType.singSingIsa, null },
            { ItemsBase.ItemType.tiil, null },
            { ItemsBase.ItemType.singSingDuha, null },
            { ItemsBase.ItemType.gamit, null },
            { ItemsBase.ItemType.armasDuha, null },
            { ItemsBase.ItemType.paaIsa, null },
            { ItemsBase.ItemType.paaDuha, null },
            { ItemsBase.ItemType.kamot, null }
        };


        private void Awake()
        {
            for (var i = 0; i < inventoryIcons.Length; i++)
            {
                inventoryIcons[i].Initialize(i + equippedNumberOffset);
            }
            equippedIconsDictionary[ItemsBase.ItemType.ulo] = equippedIcons[0];
            equippedIconsDictionary[ItemsBase.ItemType.antingAntingIsa] = equippedIcons[1];
            equippedIconsDictionary[ItemsBase.ItemType.lawas] = equippedIcons[2];
            equippedIconsDictionary[ItemsBase.ItemType.antingAntingDuha] = equippedIcons[3];
            equippedIconsDictionary[ItemsBase.ItemType.armasIsa] = equippedIcons[4];
            equippedIconsDictionary[ItemsBase.ItemType.singSingIsa] = equippedIcons[5];
            equippedIconsDictionary[ItemsBase.ItemType.tiil] = equippedIcons[6];
            equippedIconsDictionary[ItemsBase.ItemType.singSingDuha] = equippedIcons[7];
            equippedIconsDictionary[ItemsBase.ItemType.armasDuha] = equippedIcons[8];
            equippedIconsDictionary[ItemsBase.ItemType.paaIsa] = equippedIcons[9];
            equippedIconsDictionary[ItemsBase.ItemType.paaDuha] = equippedIcons[10];
            
            
            equippedIconsDictionary[ItemsBase.ItemType.ulo].Initialize(0);
            equippedIconsDictionary[ItemsBase.ItemType.antingAntingIsa].Initialize(1);
            equippedIconsDictionary[ItemsBase.ItemType.lawas].Initialize(2);
            equippedIconsDictionary[ItemsBase.ItemType.antingAntingDuha].Initialize(3);
            equippedIconsDictionary[ItemsBase.ItemType.armasIsa].Initialize(4);
            equippedIconsDictionary[ItemsBase.ItemType.singSingIsa].Initialize(5);
            equippedIconsDictionary[ItemsBase.ItemType.tiil].Initialize(6);
            equippedIconsDictionary[ItemsBase.ItemType.singSingDuha].Initialize(7);
            equippedIconsDictionary[ItemsBase.ItemType.armasDuha].Initialize(8);
            equippedIconsDictionary[ItemsBase.ItemType.paaIsa].Initialize(9);
            equippedIconsDictionary[ItemsBase.ItemType.paaDuha].Initialize(10);
            
            
        }

        private void Start()
        {
            currentNavigation = equippedNumberOffset;
            this.inventoryHighlight[currentNavigation].SetActive(true);
        }

        public void SetInventoryHighlight(List<GameObject> inventoryHighlight)
        {
            this.inventoryHighlight = inventoryHighlight;
        }
        private void Update()
        {
            HandleNavigationSelection();
        }

        public void UpdateEquippedItems(Dictionary<ItemsBase.ItemType, Item> equippedItems)
        {
            foreach (var itemType in equippedItems.Keys)
            {
                var equippedItem = equippedItems[itemType];
                if (equippedItem == null)
                {
                    equippedIconsDictionary[itemType]?.gameObject.SetActive(false);
                }
                else
                {
                    equippedIconsDictionary[itemType].UpdateItemIcon(equippedItem.itemData.icon);
                    equippedIconsDictionary[itemType].gameObject.SetActive(true);
                }
            }
        }
        
        public void UpdateInventoryItems(List<Item> inventoryItems)
        {
            for (var i = 0; i < inventoryIcons.Length; i++)
            {
                if (i < inventoryItems.Count)
                {
                    Debug.Log(i);
                    inventoryIcons[i].UpdateItemIcon(inventoryItems[i].itemData.icon);
                    inventoryIcons[i].gameObject.SetActive(true);
                }
                else
                {
                    inventoryIcons[i].gameObject.SetActive(false);
                }
            }
        }

        void HandleNavigationSelection()
        {
            bool isNavChanged = false;
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (currentNavigation < 34)
                {
                    AudioManager.i.PlaySFX(AudioId.UISelect);
                    previousNavigation = currentNavigation;
                    ++currentNavigation;
                    isNavChanged = true;
                }
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (currentNavigation > 0)
                {
                    AudioManager.i.PlaySFX(AudioId.UISelect);
                    previousNavigation = currentNavigation;
                    --currentNavigation;
                    isNavChanged = true;
                }
            }
            if (isNavChanged)
            {
                inventoryHighlight[previousNavigation].SetActive(false);
                inventoryHighlight[currentNavigation].SetActive(true);
            }

            if (Input.GetKeyDown(KeyCode.Z))
            {
                foreach (var icon in inventoryIcons)
                {
                    if (currentNavigation == icon.index)
                    {
                        AudioManager.i.PlaySFX(AudioId.UISelect);
                        icon.Selected(icon.index- equippedNumberOffset);
                    }
                }
            }else if (Input.GetKeyDown(KeyCode.X))
            {
                foreach (var icon in inventoryIcons)
                {
                    if (currentNavigation == icon.index)
                    {
                        icon.Selected(icon.index - equippedNumberOffset);
                    }
                }
            }
            
            
        }
    }
}