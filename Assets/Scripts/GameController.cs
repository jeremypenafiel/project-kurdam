using GDEUtils.StateMachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState { FreeRoam, Battle, Dialog, Paused }
public class GameController : MonoBehaviour
{
    GameState state;
    GameState stateBeforePause;
    
    public StateMachine<GameController> StateMachine { get; private set; }

    [SerializeField] PlayerController playerController;
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] Camera worldCamera;
    [SerializeField] LocationPortal locationPortal;

    Fader fader;
    public SceneDetails CurrentScene { get; private set; }
    public SceneDetails PreviousScene { get; private set; }

    public static GameController Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        fader = FindObjectOfType<Fader>();
    }
    private void Start()
    {

        StateMachine = new StateMachine<GameController>(this);
        StateMachine.ChangeState(FreeRoamState.i);

        battleSystem.OnBattleOver += EndBattle;
        battleSystem.Run += EndBattle;
        battleSystem.PlayerFaint += MovetoSpawn;
        playerController.PauseScreen += PauseGame;
        battleSystem.Pause += PauseGame;

        DialogManager.Instance.OnShowDialog += () =>
        {
            state = GameState.Dialog;
        };
        DialogManager.Instance.OnCloseDialog += () =>
        {
            if (state == GameState.Dialog)
            {
                state = GameState.FreeRoam;
            }
        };
    }

    public void StartBattle()
    {
        /*state = GameState.Battle;*/
        StateMachine.Push(BattleState1.i);
        battleSystem.gameObject.SetActive(true);
        worldCamera.gameObject.SetActive(false);

        var player = playerController.GetComponent<Player>().GetPlayer();
        var wildAswang = CurrentScene.GetComponent<MapArea>().GetRandomWildAswang();
        battleSystem.StartBattle(player, wildAswang);

    }
    public void PauseGame(bool pause)
    {
        if (pause == true)
        {
            stateBeforePause = state;
            state = GameState.Paused;


        }
        else
        {
            state = stateBeforePause;

        }
    }
    public void PauseGame()
    {
/*        if (state != GameState.Paused)
        {
            fader.FadeOut(0.5f);
            stateBeforePause = state;
            state = GameState.Paused;
            SceneManager.LoadSceneAsync(7);
            StartCoroutine(fader.FadeIn(0.5f));


        }
        else
        {
            StartCoroutine(fader.FadeIn(0.5f));

            state = stateBeforePause;
            SceneManager.LoadSceneAsync(2);
            StartCoroutine(fader.FadeOut(0.5f));

        }*/
    }

    public void PauseOnEnter()
    {
        fader.FadeOut(0.5f);
        SceneManager.LoadSceneAsync(7);
        fader.FadeIn(0.5f);
    }


    public void PauseOnExit()
    {
        fader.FadeIn(0.5f);
        SceneManager.LoadSceneAsync(2);
        fader.FadeOut(0.5f);
    }

    void EndBattle()
    {
        state = GameState.FreeRoam;
        battleSystem.gameObject.SetActive(false);
        worldCamera.gameObject.SetActive(true);
        AudioManager.i.PlayMusic(CurrentScene.SceneMusic, fade: true);
    }
    void EndBattle(bool won)
    {
        state = GameState.FreeRoam;
        battleSystem.gameObject.SetActive(false);
        worldCamera.gameObject.SetActive(true);
        AudioManager.i.PlayMusic(CurrentScene.SceneMusic, fade: true);
    }

    public void SetCurrentScene(SceneDetails currentScene)
    {
        PreviousScene = CurrentScene;
        CurrentScene = currentScene;
    }
    
    public void MovetoSpawn()
    {
        state = GameState.FreeRoam;
        battleSystem.gameObject.SetActive(false);
        worldCamera.gameObject.SetActive(true);
        locationPortal.SpawnPlayer(playerController);
    }

    private void Update()
    {

        StateMachine.Execute();

        /*if (state == GameState.Battle)
        {
            battleSystem.HandleUpdate();
        }
        else */if (state == GameState.Dialog)
        {
            DialogManager.Instance.HandleUpdate();
        }
    }


    //FOR DEBUGGING 

    private void OnGUI()
    {
        var style = new GUIStyle();
        style.fontSize = 24;
        GUILayout.Label("STATE STACK", style);
        foreach (var state in StateMachine.StateStack)
        {
            GUILayout.Label(state.GetType().ToString(), style);
        }

    }
}
