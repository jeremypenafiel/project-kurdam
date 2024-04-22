using System.Collections;
using System.Collections.Generic;
using Items;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] AswangBase playertype;
    [SerializeField] int playerlevel;
    Aswang player;
    [SerializeField] public InventorySystem inventorySystem;

    private void Start()
    {
        player = new Aswang(playertype,playerlevel);
    }

    public Aswang GetPlayer()
    {
        return player;
    }
}
