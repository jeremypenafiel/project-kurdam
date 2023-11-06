using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] AswangBase playertype;
    [SerializeField] int playerlevel;
    Aswang player;

    private void Start()
    {
        player = new Aswang(playertype,playerlevel);
    }

    public Aswang GetPlayer()
    {
        return player;
    }
}
