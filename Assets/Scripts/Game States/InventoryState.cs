using GDEUtils.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryState : State<GameController>
{

    public static InventoryState i { get; private set; }

    public void Awake()
    {
        i = this;
    }
    public override void Execute()
    {
        Debug.Log("oh wow");
    }
}
