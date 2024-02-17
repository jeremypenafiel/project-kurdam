using GDEUtils.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleState : State<GameController>
{
    [SerializeField] BattleSystem battleSystem;
    public static BattleState i { get; private set; }
    GameController gc;

    private void Awake()
    {
        i = this;
    }
    public override void Enter(GameController owner)
    {
        gc = owner;
        battleSystem.gameObject.SetActive(true);
        gc.WorldCamera.gameObject.SetActive(false);

        var player = gc.PlayerController.GetComponent<Player>().GetPlayer();
        var wildAswang = gc.CurrentScene.GetComponent<MapArea>().GetRandomWildAswang();
        battleSystem.StartBattle(player, wildAswang);

    }
    public override void Execute()
    {
        Debug.Log("BattleState");
        BattleSystem.i.HandleUpdate();
    }
}
