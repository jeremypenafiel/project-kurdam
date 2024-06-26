using System.Collections;
using GDEUtils.StateMachine;
using Items;
using UnityEngine;
using UnityEngine.Rendering.Universal;
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
/*    [SerializeField] GameObject visionLimiter;*/
    [SerializeField] InventoryView inventoryView;
    [SerializeField] Light2D globalLight;

    

    [SerializeField] public Fader fader;
    public SceneDetails CurrentScene { get; private set; }
    public SceneDetails PreviousScene { get; private set; }

    public static GameController Instance { get; private set; }

    public Camera WorldCamera => worldCamera;

    public PlayerController PlayerController => playerController;

    public Fader Fader => fader;

    public GameObject PauseScreen => pauseScreen;
    public Light2D GlobalLight =>globalLight;

/*    public GameObject VisionLimiter => visionLimiter;*/
    


    private void Awake()
    {
        Instance = this;
        QuestDB.Init();
        MovesDB.Init();
        AswangDB.Init();

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

    public IEnumerator Transition()
    {
       yield return StartCoroutine(worldCamera.GetComponent<SimpleBlit>().TransitionIn());
       
       // Pops transition state after transition coroutine finishes
       // TransitionState Exit method pushes the BattleState
       StateMachine.Pop();
       
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
        
        Debug.Log(CurrentScene.gameObject.name);
        if (PreviousScene != null)
        {
        Debug.Log(PreviousScene.gameObject.name);
            
        }
    }
    
    public void MovetoSpawn()
    {
        locationPortal.SpawnPlayer(playerController);
    }

    private void FixedUpdate()
    {
        /* StateMachine.Execute(); is called here because physics-based movement should be called in FixedUpdate
         * If in FreeRoamState, the playerController will handle the updating which includes the player movement
         * */
        /*if (StateMachine.CurrentState is FreeRoamState)
        {
            StateMachine.Execute();
        }*/

    }

    private void Update()
    {
        StateMachine.Execute();
        if (Input.GetKeyDown(KeyCode.S))
        {
            SavingSystem.i.Save("test");
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            SavingSystem.i.Load("test");
        }
        /*if (StateMachine.CurrentState is not FreeRoamState)
        {
            StateMachine.Execute();
        }*/
    }


    //FOR TESTING AND DEBUGGING 

    // private void OnGUI()
    // {
    //     var style = new GUIStyle();
    //     style.fontSize = 24;
    //     GUILayout.Label("STATE STACK", style);
    //     foreach (var state in StateMachine.StateStack)
    //     {
    //         GUILayout.Label(state.GetType().ToString(), style);
    //     }
    //
    // }

    public void StartEncounterFn()
    {
        StartBattle();
    }

    public bool IsInFreeRoamState()
    {

        return StateMachine.CurrentState is FreeRoamState;
    }
}
