using GDEUtils.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeRoamState : State<GameController>
{

    public static FreeRoamState i { get; private set; }

    public void Awake()
    {
        i = this;
    }
    public override void Execute()
    {
        PlayerController.i.HandleUpdate();
    }

}
