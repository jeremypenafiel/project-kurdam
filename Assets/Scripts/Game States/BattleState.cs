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

        // Subscribes to OnBattleOver and PlayerFaint events
        battleSystem.OnBattleOver += EndBattle;
        battleSystem.PlayerFaint += Respawn;

    }
    public override void Execute()
    {
        BattleSystem.i.HandleUpdate();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gc.StateMachine.Push(PauseGameState.i);
        }
    }

     public override void Exit()
    {
        battleSystem.gameObject.SetActive(false);
        gc.WorldCamera.gameObject.SetActive(true);

        // Unsubscribes to OnBattleOver and PlayerFaint events
        battleSystem.OnBattleOver -= EndBattle;
        battleSystem.PlayerFaint -= Respawn;
    }

    private void EndBattle()
    {
        gc.EndBattle();
        gc.StateMachine.Pop();
    }

    private void Respawn()
    {
        gc.MovetoSpawn();
    }
}
