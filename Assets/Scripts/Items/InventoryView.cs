using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Items;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using Unity.Properties;
using UnityEngine.UI;
using UnityEngine.UIElements;



public class InventoryView : StorageView
{
    private const string selectedSlotSelector = "selectedSlot";
    private const string activeDialogBoxSelector = "activeDialogBox";
    private const string activeTextSelector = "activeText";
    
    [SerializeField] string panelName = "Inventory";
    
    // Event triggered when an selected/highlighted item is changed
    // used for setting the item desciption box
    public event Action<int> OnInventoryItemSelectionChanged;
    public event Action<int> OnEquipmentItemSelectionChanged;
    public event Action<int, int> OnInventoryActionSelected;
    public event Action<int, int> OnEquipmentActionSelected;

    public event Func<int, bool> CheckIfMissionItem;
    public event Func<int, bool> CheckIfEquipmentItem;
    
    public event Action OnExitPressed;
    
    // Used for Navigation
    int currentActiveInventorySlot = 0;
    int currentActiveEquipmentSlot = 0;
    int selectedAction = 0;
    
    bool isInventoryMode = true;
    bool isCurrentItemEquipment = false;
    
    VisualElement dialogBox;
    private Label useText;
    private Label discardText;

    private Label description;
    private Label itemName;
    
    public override IEnumerator InitializeView(ViewModel viewModel)
    {
        InventorySlots = new Slot[viewModel.Capacity];
        EquipmentSlots = new Slot[6];
        root = document.rootVisualElement;
        //root.Clear();
        
        root.styleSheets.Add(styleSheet);
        
        //container =  root.CreateChild("container");
        
        // var inventory = container.CreateChild("inventory");
        // var equipment = container.CreateChild("equipment");
        // var descriptionBox = container.CreateChild("descriptionBox");
        // var dialogBox = container.CreateChild("dialogBox");
        //
        // // description stuff
        // var description = new TextElement().AddClass("descriptionText");
        // var itemName = new TextElement().AddClass("itemName");
        //
        // descriptionBox.Add(itemName);
        // descriptionBox.Add(description);
        //
        // // dialogbox stuff
        // var dialogBoxContainer = dialogBox.CreateChild("dialogBoxContainer");
        // dialogBoxContainer.Add(new Label("Use").AddClass("useText"));
        // dialogBoxContainer.Add(new Label("Discard").AddClass("discardText"));
        // dialogBox.visible = false;
        //
        // // inventory stuff
        // inventory.CreateChild("inventoryFrame");
        // inventory.CreateChild("inventoryHeader").Add(new Label(panelName));
        // var slotsContainerInventory = inventory.CreateChild("slotsContainer");
        //
        // for (var i = 0; i < viewModel.Capacity; i++)
        // {
        //     var slot = slotsContainerInventory.CreateChild<Slot>("slot");
        //     // var highlight = slot.CreateChild("highlight");
        //     InventorySlots[i] = slot;
        // }
        //
        // // equipment stuff
        // equipment.CreateChild("inventoryFrame");
        // equipment.CreateChild("inventoryHeader").Add(new Label("Equipment"));
        // var slotsContainerEquipment = equipment.CreateChild("slotsContainer");
        // for (var i = 0; i < 6; i++)
        // {
        //     var slot = slotsContainerEquipment.CreateChild<Slot>("slot");
        //     EquipmentSlots[i] = slot;
        // }
        //
        // ghostIcon = container.CreateChild("ghostIcon");
        // ghostIcon.BringToFront();

        
        var inventorySlotList = root.Q("Inventory").Q("ItemSlots").Q("VisualElement").Query<Slot>(className:"slot").ToList();
        Debug.Log(inventorySlotList.Count);
        
        for (var i = 0; i < viewModel.Capacity && i <inventorySlotList.Capacity; i++)
        {
            var slot = inventorySlotList[i];
            InventorySlots[i] = slot;
        }
        var equipmentSlotList = root.Q("Inventory").Q("ItemSlots").Q("Wearables").Query<Slot>(className:"slot").ToList();
        Debug.Log(equipmentSlotList.Count);
        for (var i = 0; i < 6 && i < equipmentSlotList.Count; i++)
        {
            var slot = equipmentSlotList[i];
            EquipmentSlots[i] = slot;
        }
        
        
        dialogBox = root.Q("DescRow").Q(className: "dialogBox");
        dialogBox.visible = false;
        
        description = root.Q("DescRow").Q(className:"descriptionBox").Q<Label>(className:"description");
        itemName = root.Q("DescRow").Q(className:"descriptionBox").Q<Label>(className:"itemName");
        
        useText = dialogBox.Q<Label>(className: "useText");
        discardText = dialogBox.Q<Label>(className: "discardText");
        
        Debug.Assert(dialogBox != null, "dialogBox == null");
        Debug.Assert(description != null, "description == null");
        Debug.Assert(itemName != null, "itemName == null");
        Debug.Assert(useText != null, "useText == null");
        Debug.Assert(discardText!= null, "discardText == null");
        // InventorySlots[currentActiveInventorySlot].AddClass(selectedSlotSelector);
        OnInventoryItemSelectionChanged?.Invoke(currentActiveInventorySlot);
        

        yield return null;
    }


    void HandleInventoryNavigationSelection()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            //AudioManager.PlaySFX(AudioId.UISelect);
            InventorySlots[currentActiveInventorySlot].RemoveFromClassList(selectedSlotSelector);
            currentActiveInventorySlot = (currentActiveInventorySlot + 1) % InventorySlots.Length ;
            InventorySlots[currentActiveInventorySlot].AddClass(selectedSlotSelector);
            OnInventoryItemSelectionChanged?.Invoke(currentActiveInventorySlot);
        }else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            //AudioManager.PlaySFX(AudioId.UISelect);
            if (currentActiveInventorySlot == 0)
            {
                InventorySlots[currentActiveInventorySlot].RemoveFromClassList(selectedSlotSelector);
                currentActiveInventorySlot = InventorySlots.Length - 1 ;
                InventorySlots[currentActiveInventorySlot].AddClass(selectedSlotSelector);
            } else
            {
                InventorySlots[currentActiveInventorySlot].RemoveFromClassList(selectedSlotSelector);
                currentActiveInventorySlot = (currentActiveInventorySlot - 1) % InventorySlots.Length ;
                InventorySlots[currentActiveInventorySlot].AddClass(selectedSlotSelector);
            }
            OnInventoryItemSelectionChanged?.Invoke(currentActiveInventorySlot);
        }else if (Input.GetKeyDown(KeyCode.Z))
        {
            if(InventorySlots[currentActiveInventorySlot].ItemId == SerializableGuid.Empty) return;
            if (CheckIfMissionItem!.Invoke(currentActiveInventorySlot)) return;

            if (CheckIfEquipmentItem!.Invoke(currentActiveInventorySlot))
            {
                useText.text = "Equip";
                isCurrentItemEquipment = true;
            }
            else
            {
                useText.text = "Use";
                isCurrentItemEquipment = false;
            }
            
            //AudioManager.PlaySFX(AudioId.UISelect);
            dialogBox.visible = true;
            useText.style.color = Color.blue;

        }else if (Input.GetKeyDown(KeyCode.E))
        {
            isInventoryMode = !isInventoryMode;
            EquipmentSlots[currentActiveEquipmentSlot].AddClass(selectedSlotSelector);
            OnEquipmentItemSelectionChanged?.Invoke(currentActiveEquipmentSlot);
        }else if (Input.GetKeyDown(KeyCode.X))
        {
            OnExitPressed?.Invoke();
        }
    }

    private void Update()
    {
        if (!dialogBox.visible)
        {
            if(isInventoryMode) HandleInventoryNavigationSelection();
            else HandleEquipmentNavigationSelection();
        }
        else
        {
            HandleActionSelection();
        }
    }

    private void HandleEquipmentNavigationSelection()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            //AudioManager.PlaySFX(AudioId.UISelect);
            EquipmentSlots[currentActiveEquipmentSlot].RemoveFromClassList(selectedSlotSelector);
            currentActiveEquipmentSlot = (currentActiveEquipmentSlot + 1) % EquipmentSlots.Length ;
            EquipmentSlots[currentActiveEquipmentSlot].AddClass(selectedSlotSelector);
            OnEquipmentItemSelectionChanged?.Invoke(currentActiveEquipmentSlot);
        }else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            //AudioManager.PlaySFX(AudioId.UISelect);
            if (currentActiveEquipmentSlot == 0)
            {
                EquipmentSlots[currentActiveEquipmentSlot].RemoveFromClassList(selectedSlotSelector);
                currentActiveEquipmentSlot = EquipmentSlots.Length - 1 ;
                EquipmentSlots[currentActiveEquipmentSlot].AddClass(selectedSlotSelector);
            } else
            {
                EquipmentSlots[currentActiveEquipmentSlot].RemoveFromClassList(selectedSlotSelector);
                currentActiveEquipmentSlot = (currentActiveEquipmentSlot - 1) % EquipmentSlots.Length ;
                EquipmentSlots[currentActiveEquipmentSlot].AddClass(selectedSlotSelector);
            }
            OnEquipmentItemSelectionChanged?.Invoke(currentActiveEquipmentSlot);
        }else if (Input.GetKeyDown(KeyCode.Z))
        {
            if(EquipmentSlots[currentActiveEquipmentSlot].ItemId == SerializableGuid.Empty) return;
            
            
            //AudioManager.PlaySFX(AudioId.UISelect);
            dialogBox.visible = true;
            
            useText.style.color = Color.blue;
            useText.text = "Unequip";

        }else if (Input.GetKeyDown(KeyCode.E))
        {
            isInventoryMode = !isInventoryMode;
            OnInventoryItemSelectionChanged?.Invoke(currentActiveInventorySlot);
        }else if (Input.GetKeyDown(KeyCode.X))
        {
            OnExitPressed?.Invoke();
        }
    }

    private void HandleActionSelection()
    { 
        
        
        
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            //AudioManager.PlaySFX(AudioId.UISelect);
            selectedAction = ~selectedAction;
        }else if (Input.GetKeyDown(KeyCode.X))
        {
            //AudioManager.PlaySFX(AudioId.UISelect);
            dialogBox.visible = false;
        }

        if (selectedAction == 0)
        {
            useText.style.color = Color.blue;
            discardText.style.color = Color.black;
        }
        else
        {
            useText.style.color = Color.black;
            discardText.style.color = Color.blue;
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (isInventoryMode)
            {
                OnInventoryActionSelected?.Invoke(selectedAction, currentActiveInventorySlot);
            }
            else
            {
                OnEquipmentActionSelected?.Invoke(selectedAction, currentActiveEquipmentSlot);
            }
            dialogBox.visible = false;
        }
    }


    public void SetItemDescriptionBox([CanBeNull] Item item)
    {
        if (item == null)
        {
            description.text = "";
            itemName.text = "";
            return;
        }
        
        description.text = item.details.description;
        itemName.text = item.details.name;
    }
}
