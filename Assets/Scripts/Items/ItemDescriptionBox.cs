using TMPro;
using UnityEngine;

namespace Items
{
    public class ItemDescriptionBox:MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI itemName;
        [SerializeField] private TextMeshProUGUI itemDescription;
        [SerializeField] private TextMeshProUGUI itemAmount;
        
        public void SetItemDescription(string iName, string description, string amount)
        {
            itemName.text = iName;
            itemDescription.text = description;
            itemAmount.text = amount;
        }
        
    }
}