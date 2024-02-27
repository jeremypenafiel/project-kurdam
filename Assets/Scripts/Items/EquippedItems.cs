using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquippedItems : MonoBehaviour
{
    [SerializeField] ItemsBase armas;
    [SerializeField] ItemsBase ulo;
    [SerializeField] ItemsBase antingAnting;
    [SerializeField] ItemsBase singsing;
    [SerializeField] ItemsBase lawas;
    [SerializeField] ItemsBase paa;
    [SerializeField] ItemsBase tiil;
    [SerializeField] ItemsBase kamot;






    public ItemsBase Armas { get { return armas; } }
    public ItemsBase Ulo { get {  return ulo; } }
    public ItemsBase AntingAnting { get {  return antingAnting; } }
    public ItemsBase Singsing { get { return singsing; } }  

    public ItemsBase Lawas { get {  return lawas; } }
    public ItemsBase Kamot { get {  return kamot; } }
    public ItemsBase Tiil { get {  return tiil; } }
    public ItemsBase Paa { get { return paa; } }
}
