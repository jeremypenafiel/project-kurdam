using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Create new item")]

public class ItemsBase : ScriptableObject
{
    [SerializeField] string iname;
    [SerializeField] string type;
    [SerializeField] DamageType weaponType;
    [SerializeField] List<Aswang> advantage;
    [SerializeField] List<Aswang> disadvantage;
    [SerializeField] Sprite icon;
    [SerializeField] int armorClass;
    [SerializeField] int damageModifier;
    [SerializeField] List<string> Resistances;

    public string Iname
    {
        get { return iname; }
    }

    public string Type
    {
        get { return type; }
    }

    public DamageType WeaponType
    {
        get { return weaponType; }
    }

    public List<Aswang> Advantage
    {
        get { return advantage; }
    }

    public List<Aswang> Disadvantage
    {
        get { return disadvantage; }
    }
    public Sprite Icon
    {
        get { return icon; }
    }

    public int ArmorClass
    {
        get { return armorClass; }
    }

    public int DamageModifier
    {
        get { return damageModifier; }
    }
}
