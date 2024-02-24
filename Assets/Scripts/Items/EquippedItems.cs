using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquippedItems : MonoBehaviour
{
    [SerializeField] Dictionary<string, ItemsBase> equipments;
    [SerializeField] ItemsBase weapon;
 

    public DamageType Type
    {
        get { return weapon.WeaponType; }
    }
}
