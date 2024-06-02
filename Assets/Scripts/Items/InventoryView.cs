using System;
using System.Collections;
using Items;
using JetBrains.Annotations;
using UnityEngine;
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
    bool isDialogBoxActive = false;
    
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
        
        root.styleSheets.Add(styleSheet);
        
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
        InventorySlots[currentActiveInventorySlot].AddClass(selectedSlotSelector);
        OnInventoryItemSelectionChanged?.Invoke(currentActiveInventorySlot);
        

        yield return null;
    }


    void HandleInventoryNavigationSelection()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            AudioManager.i.PlaySFX(AudioId.UISelect);
            InventorySlots[currentActiveInventorySlot].RemoveFromClassList(selectedSlotSelector);
            currentActiveInventorySlot = (currentActiveInventorySlot + 1) % InventorySlots.Length ;
            InventorySlots[currentActiveInventorySlot].AddClass(selectedSlotSelector);
            OnInventoryItemSelectionChanged?.Invoke(currentActiveInventorySlot);
        }else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            AudioManager.i.PlaySFX(AudioId.UISelect);
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

            //AudioManager.PlaySFX(AudioId.UISelect);
            isDialogBoxActive = true;

        }else if (Input.GetKeyDown(KeyCode.E))
        {
            isInventoryMode = !isInventoryMode;
            InventorySlots[currentActiveInventorySlot].RemoveFromClassList(selectedSlotSelector);
            currentActiveInventorySlot = 0;
            EquipmentSlots[currentActiveEquipmentSlot].AddClass(selectedSlotSelector);
            OnEquipmentItemSelectionChanged?.Invoke(currentActiveEquipmentSlot);
        }else if (Input.GetKeyDown(KeyCode.X))
        {
            OnExitPressed?.Invoke();
        }
    }

    private void Update()
    {
        if (!isDialogBoxActive)
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
            AudioManager.i.PlaySFX(AudioId.UISelect);
            EquipmentSlots[currentActiveEquipmentSlot].RemoveFromClassList(selectedSlotSelector);
            currentActiveEquipmentSlot = (currentActiveEquipmentSlot + 1) % EquipmentSlots.Length ;
            EquipmentSlots[currentActiveEquipmentSlot].AddClass(selectedSlotSelector);
            OnEquipmentItemSelectionChanged?.Invoke(currentActiveEquipmentSlot);
        }else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            AudioManager.i.PlaySFX(AudioId.UISelect);
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
            isDialogBoxActive= true;
            
            useText.style.color = Color.blue;

        }else if (Input.GetKeyDown(KeyCode.E))
        {
            EquipmentSlots[currentActiveEquipmentSlot].RemoveFromClassList(selectedSlotSelector);
            currentActiveEquipmentSlot = 0;
            isInventoryMode = !isInventoryMode;
            InventorySlots[currentActiveInventorySlot].AddClass(selectedSlotSelector);
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
            AudioManager.i.PlaySFX(AudioId.UISelect);
            selectedAction = ~selectedAction;
        }else if (Input.GetKeyDown(KeyCode.X))
        {
            //AudioManager.PlaySFX(AudioId.UISelect);
            useText.style.color = Color.black;
            discardText.style.color = Color.black;
            isDialogBoxActive = false;
            return;
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
            isDialogBoxActive = false;
        }
    }


    public void SetItemDescriptionBox([CanBeNull] Item item)
    {
        if (item == null)
        {
            description.text = "";
            itemName.text = "";
            useText.text = "Use";
            return; 
        }
        useText.text = item.details.isConsumable ? "Use" : isInventoryMode ? "Equip" : "Unequip";
        discardText.text = "Discard";
        description.text = item.details.description;
        itemName.text = item.details.name;
    }
}
