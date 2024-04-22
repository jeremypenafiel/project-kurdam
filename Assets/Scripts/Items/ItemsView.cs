using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Items
{
    public class ItemsView : MonoBehaviour
    {
        [SerializeField] List<GameObject> inventoryHighlight;
        [SerializeField] public ItemIcon[] equippedIcons;
        [SerializeField] public ItemIcon[] inventoryIcons;
        [SerializeField] public InventoryDialogueBox inventoryDialogueBox;
        [SerializeField] public ItemDescriptionBox itemDescriptionBox;
        private int equippedNumberOffset = 11;
        private int currentNavigation;
        private int previousNavigation;
        private int currentActionSelection = 0;
        private Item currentItem = null;
        private event Func<int, Item> onSelectionChanged;
        private event Action onInventoryExit;

        [SerializeField] public Dictionary<EquippableItemsBase.ItemType, ItemIcon> equippedIconsDictionary = new()
        {
            
            { EquippableItemsBase.ItemType.ulo, null },
            { EquippableItemsBase.ItemType.antingAntingIsa, null },
            { EquippableItemsBase.ItemType.lawas, null },
            { EquippableItemsBase.ItemType.antingAntingDuha, null },
            { EquippableItemsBase.ItemType.armasIsa, null },
            { EquippableItemsBase.ItemType.singSingIsa, null },
            { EquippableItemsBase.ItemType.tiil, null },
            { EquippableItemsBase.ItemType.singSingDuha, null },
            { EquippableItemsBase.ItemType.gamit, null },
            { EquippableItemsBase.ItemType.armasDuha, null },
            { EquippableItemsBase.ItemType.paaIsa, null },
            { EquippableItemsBase.ItemType.paaDuha, null },
            { EquippableItemsBase.ItemType.kamot, null }
        };


        private void Awake()
        {
            for (var i = 0; i < inventoryIcons.Length; i++)
            {
                inventoryIcons[i].Initialize(i + equippedNumberOffset);
            }
            equippedIconsDictionary[EquippableItemsBase.ItemType.ulo] = equippedIcons[0];
            equippedIconsDictionary[EquippableItemsBase.ItemType.antingAntingIsa] = equippedIcons[1];
            equippedIconsDictionary[EquippableItemsBase.ItemType.lawas] = equippedIcons[2];
            equippedIconsDictionary[EquippableItemsBase.ItemType.antingAntingDuha] = equippedIcons[3];
            equippedIconsDictionary[EquippableItemsBase.ItemType.armasIsa] = equippedIcons[4];
            equippedIconsDictionary[EquippableItemsBase.ItemType.singSingIsa] = equippedIcons[5];
            equippedIconsDictionary[EquippableItemsBase.ItemType.tiil] = equippedIcons[6];
            equippedIconsDictionary[EquippableItemsBase.ItemType.singSingDuha] = equippedIcons[7];
            equippedIconsDictionary[EquippableItemsBase.ItemType.armasDuha] = equippedIcons[8];
            equippedIconsDictionary[EquippableItemsBase.ItemType.paaIsa] = equippedIcons[9];
            equippedIconsDictionary[EquippableItemsBase.ItemType.paaDuha] = equippedIcons[10];
            
            
            equippedIconsDictionary[EquippableItemsBase.ItemType.ulo].Initialize(0);
            equippedIconsDictionary[EquippableItemsBase.ItemType.antingAntingIsa].Initialize(1);
            equippedIconsDictionary[EquippableItemsBase.ItemType.lawas].Initialize(2);
            equippedIconsDictionary[EquippableItemsBase.ItemType.antingAntingDuha].Initialize(3);
            equippedIconsDictionary[EquippableItemsBase.ItemType.armasIsa].Initialize(4);
            equippedIconsDictionary[EquippableItemsBase.ItemType.singSingIsa].Initialize(5);
            equippedIconsDictionary[EquippableItemsBase.ItemType.tiil].Initialize(6);
            equippedIconsDictionary[EquippableItemsBase.ItemType.singSingDuha].Initialize(7);
            equippedIconsDictionary[EquippableItemsBase.ItemType.armasDuha].Initialize(8);
            equippedIconsDictionary[EquippableItemsBase.ItemType.paaIsa].Initialize(9);
            equippedIconsDictionary[EquippableItemsBase.ItemType.paaDuha].Initialize(10);
            
            
        }

        public void ActivateCamera()
        {
            gameObject.GetComponent<Canvas>().worldCamera.gameObject.SetActive(true);
            gameObject.SetActive(true);
        }

        private void Start()
        {
            currentNavigation = equippedNumberOffset;
            inventoryHighlight[currentNavigation].SetActive(true);
            

            SetItemDescriptionTexts();
            
        }

        public void RegisterSelectionListener(Func<int, Item> listener)
        {
            onSelectionChanged += listener;
        }

        public void RegisterExitListener(Action listener)
        {
            onInventoryExit += listener;
        }
        
        public void RemoveExitListener(Action listener)
        {
            onInventoryExit -= listener;
        }

        public void SetInventoryHighlight(List<GameObject> inventoryHighlight)
        {
            this.inventoryHighlight = inventoryHighlight;
        }

        public void EnableDialogBox(bool isEquip)
        {
            inventoryDialogueBox.SetEquipText(isEquip);
            inventoryDialogueBox.gameObject.SetActive(true);
        }
        
        private void Update()
        {
            bool isDialogBoxActive = inventoryDialogueBox.gameObject.activeSelf;
            if (!isDialogBoxActive)
            {
                HandleNavigationSelection();
            }
            else
            {
                HandleActionSelection();
            }
        }

        public void UpdateEquippedItems(Dictionary<EquippableItemsBase.ItemType, EquippableItem> equippedItems)
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
            
            SetItemDescriptionTexts();
        }
        
        public void UpdateInventoryItems(List<Item> inventoryItems)
        {
            for (var i = 0; i < inventoryIcons.Length; i++)
            {
                if (i < inventoryItems.Count)
                {
                    inventoryIcons[i].UpdateItemIcon(inventoryItems[i].itemData.icon);
                    inventoryIcons[i].gameObject.SetActive(true);
                }
                else
                {
                    inventoryIcons[i].gameObject.SetActive(false);
                }
            }
            
            SetItemDescriptionTexts();
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
                SetItemDescriptionTexts();
                inventoryHighlight[previousNavigation].SetActive(false);
                inventoryHighlight[currentNavigation].SetActive(true);
            }

            if (Input.GetKeyDown(KeyCode.Z))
            {
                foreach (var icon in inventoryIcons)
                {
                    if (currentNavigation == icon.index && icon.gameObject.activeSelf)
                    {
                        AudioManager.i.PlaySFX(AudioId.UISelect);
                        icon.Selected(icon.index- equippedNumberOffset);
                    }
                }
            }else if (Input.GetKeyDown(KeyCode.X))
            {
                onInventoryExit?.Invoke();
            }
        }

        private void SetItemDescriptionTexts()
        {
            currentItem = onSelectionChanged?.Invoke(currentNavigation-equippedNumberOffset);
            if (currentItem is null)
            {
                itemDescriptionBox.SetItemDescription("", "", "");
            }
            else
            {
                if (currentItem is ConsumableItem item)
                {
                    itemDescriptionBox.SetItemDescription(item.itemData.itemName, item.itemData.description, $"Amount: {item.Amount.ToString()}");
                    return;
                }
                itemDescriptionBox.SetItemDescription(currentItem.itemData.itemName, currentItem.itemData.description, "");
            }
        }

        void HandleActionSelection()
        {
            var currentIconIndex = currentNavigation - equippedNumberOffset;
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                currentActionSelection = 0;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                currentActionSelection = 1;
            }
            
            inventoryDialogueBox.UpdateActionSelection(currentActionSelection);
            
            if (Input.GetKeyDown(KeyCode.Z))
            {
                Debug.Log("z works");
                inventoryIcons[currentIconIndex].ActionSelected(currentIconIndex, currentActionSelection);
                inventoryDialogueBox.gameObject.SetActive(false);
            }else if (Input.GetKeyDown(KeyCode.X))
            {
                inventoryDialogueBox.gameObject.SetActive(false);
            }
        }
    }
}