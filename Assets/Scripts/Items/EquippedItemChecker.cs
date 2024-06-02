using Items;
using UnityEngine;

public class EquippedItemChecker : MonoBehaviour
{
    private Aswang player; // Reference to the player object or wherever you manage equipped items

    private void Start()
    {
        // Get reference to the player object or wherever you manage equipped items
        player = FindObjectOfType<Player>().GetPlayer(); // Adjust this based on your actual implementation

        CheckForSuga();
    }

    private void CheckForSuga()
    {
        if (player.EquippedItems.ContainsKey(EquippableItemsBase.ItemType.suga))
        {
            Debug.Log("Suga equipped!");
            // Do something when suga is equipped
        }
        else
        {
            Debug.Log("Suga not equipped.");
            // Do something when suga is not equipped
        }
    }
}
