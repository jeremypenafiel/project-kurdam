using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Create new item")]

public class ItemsBase : ScriptableObject
{
    [SerializeField] public string itemName;
    [SerializeField] public ItemType type;

    [SerializeField] public DamageType armasType;
    [SerializeField] public List<Aswang> advantage;
    [SerializeField] public List<Aswang> disadvantage;
    [SerializeField] public Sprite icon;
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
