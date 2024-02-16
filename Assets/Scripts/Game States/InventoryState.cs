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

    GameController gc;


    public override void Enter(GameController owner)
    {
        gc = owner;
    }
    public override void Execute()
    {
        if(Input.GetKeyDown(KeyCode.X))
        {
            gc.StateMachine.Pop();
        }
    }
}
