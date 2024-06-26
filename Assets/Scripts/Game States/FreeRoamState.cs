using GDEUtils.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeRoamState : State<GameController>
{

    public static FreeRoamState i { get; private set; }

    GameController gc;


    public void Awake()
    {
        i = this;
    }

    public override void Enter(GameController owner)
    {
        gc = owner;
    }

    public override void Execute()
    {
        PlayerController.i.HandleUpdate();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gc.StateMachine.Push(PauseGameState.i);
        }else if (Input.GetKeyDown(KeyCode.I))
        {
            AudioManager.i.PlaySFX(AudioId.UISelect);
            gc.StateMachine.Push(InventoryState.i);
        }else if (Input.GetKeyDown(KeyCode.M))
        {
            AudioManager.i.PlaySFX(AudioId.UISelect);
            gc.StateMachine.Push(MovesChangeState.i);
        }
    }

}
