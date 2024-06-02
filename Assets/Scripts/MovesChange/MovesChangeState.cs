using GDEUtils.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MovesChangeState : State<GameController>
{

    [SerializeField] MovesChangeSystem movesChangeSystem;
    public static MovesChangeState i { get; private set; }
    GameController gc;

    private void Awake()
    {
        i = this;
    }

    public override void Enter(GameController owner)
    {
        gc = owner;
        /*StartCoroutine(gc.Transition());*/
        movesChangeSystem.gameObject.SetActive(true);
        gc.WorldCamera.gameObject.SetActive(false);
        /*        gc.VisionLimiter.SetActive(false);*/
        movesChangeSystem.Exit += Close;
        var player = gc.PlayerController.GetComponent<Player>().GetPlayer();



        movesChangeSystem.StartBattle(player);

        // Subscribes to OnBattleOver and PlayerFaint events


    }
    public override void Execute()
    {
        MovesChangeSystem.i.HandleUpdate();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gc.StateMachine.Push(PauseGameState.i);
        }
    }

    public override void Exit()
    {
        movesChangeSystem.gameObject.SetActive(false);
        gc.WorldCamera.gameObject.SetActive(true);
        /*        gc.VisionLimiter.SetActive(true);*/
        movesChangeSystem.Exit -= Close;
        
        // Unsubscribes to OnBattleOver and PlayerFaint events
 
    }

    private void Close()
    {
        gc.StateMachine.Pop();
    }


}
