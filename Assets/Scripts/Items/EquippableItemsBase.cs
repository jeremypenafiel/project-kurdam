using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    [CreateAssetMenu(fileName = "Item", menuName = "Items/EquippableItem")]
    public class EquippableItemsBase: ItemsBase
    {
        [SerializeField] public ItemType type;
        [SerializeField] public DamageType armasType;
        [SerializeField] public List<Aswang> advantage;
        [SerializeField] public List<Aswang> disadvantage;
        [SerializeField] public int armorClass;
        [SerializeField] public int damageModifier;
        [SerializeField] public List<string> Resistances;
    

        public enum ItemType
        {
            armasIsa,
            armasDuha,
            ulo,
            antingAntingIsa,
            antingAntingDuha,
            singSingIsa,
            singSingDuha,
            lawas,
            paaIsa,
            paaDuha,
            tiil,
            kamot,
            gamit 
        }
    }
}