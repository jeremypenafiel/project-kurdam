using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GDEUtils;
using GDEUtils.StateMachine;

public class DialogState : State<GameController>
{
    GameController gc;
    public static DialogState i { get; private set; }


    private void Awake()
    {
        i = this;
    }

    public override void Enter(GameController owner)
    {
        gc = owner;
    }

    public override void Execute()
    {
        DialogManager.Instance.HandleUpdate();
    }


}
