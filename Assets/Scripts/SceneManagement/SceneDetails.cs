using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class SceneDetails : MonoBehaviour
{
    [SerializeField] List<SceneDetails> connectedScenes;
    [SerializeField] AudioClip sceneMusic;
    [SerializeField] /*GameObject dialogBox;
    [SerializeField] TextMeshProUGUI sceneNameText;*/


    GameObject sceneNamePopUp;
    TextMeshProUGUI sceneNameText;
    Vector3 origPos;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        sceneNamePopUp = GameObject.Find("SceneName");
        sceneNameText = sceneNamePopUp.GetComponentInChildren<TextMeshProUGUI>();
        origPos = sceneNamePopUp.transform.localPosition;
    }

    public bool IsLoaded { get; private  set; } = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Debug.Log($"Entered {gameObject.name}");

            StartCoroutine(LoadScene());
            if(sceneNamePopUp == null)
            {
                Init();
            }
            GameController.Instance.SetCurrentScene(this);
            switch (gameObject.name)
            {
                case "LoversLane": 
                     sceneNameText.text = "Lovers' Lane";
                    break;
                case "CAS":
                    sceneNameText.text = "CAS";
                    break;
                case "HSU":
                    sceneNameText.text = "HSU";
                    break;
                case "NewAdmin":
                    sceneNameText.text = "New Admin";
                    break;
                case "CL4":
                    sceneNameText.text = "CL4";
                    break;
                case "NatureTrail":
                    sceneNameText.text = "Nature Trail";
                    break;
                default:
                    Debug.Log("Scene name not found");
                    Debug.Log(sceneNamePopUp.name);
                    break;
            }
            PlayPopUpAnimation();

            if (sceneMusic != null)
            {
                AudioManager.i.PlayMusic(sceneMusic, fade: true);
            }

            foreach(var scene in connectedScenes)
            {
                StartCoroutine(scene.LoadScene());
            }

            var previousScene = GameController.Instance.PreviousScene;

            if(previousScene != null)
            {
                var previouslyLoadedScenes = GameController.Instance.PreviousScene.connectedScenes;
                foreach(var scene in previouslyLoadedScenes)
                {
                    if(!connectedScenes.Contains(scene) && scene != this)
                    {
                        Debug.Log("diri");
                        scene.UnloadScene();
                    }   
                }   

                if(!connectedScenes.Contains(previousScene))
                {
                        Debug.Log("here");
                    previousScene.UnloadScene();
                }
            }



        }
    }

    public IEnumerator LoadScene()
    {
        if (!IsLoaded)
        {
            AsyncOperation asyncLoad =  SceneManager.LoadSceneAsync(gameObject.name, LoadSceneMode.Additive);

            
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
            IsLoaded = true;
        }
    }

    public void UnloadScene()
    {
        
        if (IsLoaded)
        {
            Debug.Log($"Unloading {gameObject.name}");
            SceneManager.UnloadSceneAsync(gameObject.name);
            IsLoaded = false;
        }
    }
    public void PlayPopUpAnimation()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(sceneNamePopUp?.transform.DOLocalMoveY(origPos.y - 100f, 0.5f));
        sequence.AppendInterval(2.5f);
        sequence.Append(sceneNamePopUp?.transform.DOLocalMoveY(origPos.y, 0.5f));
    }



    public AudioClip SceneMusic => sceneMusic;
}