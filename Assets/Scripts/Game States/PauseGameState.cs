using GDEUtils.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGameState : State<GameController>
{
    public static PauseGameState i { get; private set; }

    public void Awake()
    {
        i = this;
    }

    GameController gc;

    public override void Enter(GameController owner)
    {
        gc = owner;
        gc.PauseOnEnter();
    }
    public override void Execute()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            gc.StateMachine.Pop();
        }
    }

    public override void Exit()
    {
        gc.PauseOnExit();
    }
}

