using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Equipment")]
public class EquippedItems : ScriptableObject   
{
    [SerializeField] ItemData armas;
    [SerializeField] ItemData ulo;
    [SerializeField] ItemData antingAnting;
    [SerializeField] ItemData singsing;
    [SerializeField] ItemData lawas;
    [SerializeField] ItemData paa;
    [SerializeField] ItemData tiil;
    [SerializeField] ItemData kamot;



    public ItemData Armas { get { return armas; } }
    public ItemData Ulo { get {  return ulo; } }
    public ItemData AntingAnting { get {  return antingAnting; } }
    public ItemData Singsing { get { return singsing; } }  

    public ItemData Lawas { get {  return lawas; } }
    public ItemData Kamot { get {  return kamot; } }
    public ItemData Tiil { get {  return tiil; } }
    public ItemData Paa { get { return paa; } }
}
