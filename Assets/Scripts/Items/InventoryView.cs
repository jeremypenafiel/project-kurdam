using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InventoryView : StorageView
{
    
    [SerializeField] string panelName = "Inventory";
    
    

    public override IEnumerator InitializeView(int size = 20)
    {
        Slots = new Slot[size];
        root = document.rootVisualElement;
        root.Clear();
        
        root.styleSheets.Add(styleSheet);
        
        container =  root.CreateChild("container");
        
        
        var inventory = container.CreateChild("inventory");
        inventory.CreateChild("inventoryFrame");
        inventory.CreateChild("inventoryHeader").Add(new Label(panelName));

        var slotsContainer = inventory.CreateChild("slotsContainer");
        for (int i = 0; i < size; i++)
        {
            var slot = slotsContainer.CreateChild<Slot>("slot");
            // var highlight = slot.CreateChild("highlight");
            Slots[i] = slot;
        }

        ghostIcon = container.CreateChild("ghostIcon");
        ghostIcon.BringToFront();

        Slots[0].AddClass("active");

        yield return null;
    }

    
}
