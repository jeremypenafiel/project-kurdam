using TMPro;
using UnityEngine;

namespace Items
{
    public class InventoryDialogueBox : MonoBehaviour
    {
        [SerializeField] private Color highlightedColor;
        [SerializeField] TextMeshProUGUI promptText;
        [SerializeField] TextMeshProUGUI equipText;
        [SerializeField] TextMeshProUGUI discardText;


        public void SetEquipText(bool isEquip)
        {
            equipText.text = isEquip ? "Equip" : "Use";
        }

        public void UpdateActionSelection(int selectedAction)
        {
            switch (selectedAction)
            {
                case 0:
                    equipText.color = highlightedColor;
                    discardText.color = Color.black;
                    break;
                case 1:
                    discardText.color = highlightedColor;
                    equipText.color = Color.black;
                    break;
            }
        }
    }
}