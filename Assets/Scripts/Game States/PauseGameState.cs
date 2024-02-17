using GDEUtils.StateMachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        //gc.PauseOnEnter();
        gc.Fader.FadeIn(0.5f);
        gc.PauseScreen.SetActive(true);
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

        gc.Fader.FadeIn(0.5f);
        gc.PauseScreen.gameObject.SetActive(false);
        gc.Fader.FadeOut(0.5f);
    }
}

