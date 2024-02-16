using GDEUtils.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleState1 : State<GameController>
{
    public static BattleState1 i { get; private set; }
    GameController gc;

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
        Debug.Log("BattleState");
        BattleSystem.i.HandleUpdate();
    }
}
