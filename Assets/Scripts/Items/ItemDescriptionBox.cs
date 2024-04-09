using TMPro;
using UnityEngine;

namespace Items
{
    public class ItemDescriptionBox:MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI itemName;
        [SerializeField] private TextMeshProUGUI itemDescription;
        
        
        public void SetItemDescription(string iName, string description)
        {
            itemName.text = iName;
            itemDescription.text = description;
        }
        
    }
}