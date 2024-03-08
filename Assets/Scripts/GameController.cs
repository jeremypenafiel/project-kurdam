using GDEUtils.StateMachine;
using System;
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
    [SerializeField] GameObject pauseScreen;
    [SerializeField] GameObject visionLimiter;

    Fader fader;
    public SceneDetails CurrentScene { get; private set; }
    public SceneDetails PreviousScene { get; private set; }

    public static GameController Instance { get; private set; }

    public Camera WorldCamera => worldCamera;

    public PlayerController PlayerController => playerController;

    public Fader Fader => fader;

    public GameObject PauseScreen => pauseScreen;

    public GameObject VisionLimiter => visionLimiter;  

    private void Awake()
    {
        Instance = this;
        fader = FindObjectOfType<Fader>();
    }
    private void Start()
    {

        StateMachine = new StateMachine<GameController>(this);
        StateMachine.ChangeState(FreeRoamState.i);


        // DialogState is pushed to the state stack if DialogManager.OnShowDialog runs
        DialogManager.Instance.OnShowDialog += () =>
        {
            StateMachine.Push(DialogState.i);
        };

        // DialogState is popped from the state stack if DialogManager.OnCloseDialog runs
        DialogManager.Instance.OnCloseDialog += () =>
        {
            StateMachine.Pop();
        };
    }

    public void StartBattle()
    {
        StateMachine.Push(BattleState.i);

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

   public void EndBattle()
    {

        AudioManager.i.PlayMusic(CurrentScene.SceneMusic, fade: true);
    }


    public void SetCurrentScene(SceneDetails currentScene)
    {
        PreviousScene = CurrentScene;
        CurrentScene = currentScene;
    }
    
    public void MovetoSpawn()
    {
        locationPortal.SpawnPlayer(playerController);
    }

    private void Update()
    {

        StateMachine.Execute();

    }


    //FOR TESTING AND DEBUGGING 

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

    public void StartEncounterFn()
    {
        StartBattle();
    }

    public bool IsInFreeRoamState()
    {

        return StateMachine.CurrentState is FreeRoamState;
    }
}
