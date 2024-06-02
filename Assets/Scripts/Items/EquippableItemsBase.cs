using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    [CreateAssetMenu(fileName = "New Equippable Item", menuName = "Items/EquippableItem")]
    public class EquippableItemsBase: ItemsBase
    {
        [SerializeField] public ItemType type;
        [SerializeField] public DamageType armasType;
        [SerializeField] public List<Aswang> advantage;
        [SerializeField] public List<Aswang> disadvantage;
        [SerializeField] public int armorClass;
        [SerializeField] public int damageModifier;
        [SerializeField] public List<string> Resistances;
        [SerializeField] public float lightIntensity;
        [SerializeField] public Color lightTemperature;
    

        public enum ItemType
        {
            armasIsa,
            suga,
            antingAnting,
            lawas,
            paa,
            tiil,
        }



        public override Item Create(int quantity)
        {
            return new EquippableItem(this, quantity);
        }
    }
}