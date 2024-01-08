using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SceneDetails : MonoBehaviour
{
    [SerializeField] List<SceneDetails> connectedScenes;
    [SerializeField] AudioClip sceneMusic;

    GameObject sceneNamePopUp;
    TextMeshProUGUI sceneNameText;

/*    private void Start()
    {
        
    }*/

    public bool IsLoaded { get; private  set; } = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Debug.Log($"Entered {gameObject.name}");

            LoadScene();
            GameController.Instance.SetCurrentScene(this);

            sceneNamePopUp?.SetActive(true);
            /*sceneNameText.text = gameObject.name;*/



            if(sceneMusic != null)
            {
                AudioManager.i.PlayMusic(sceneMusic, fade: true);
            }

            foreach(var scene in connectedScenes)
            {
                scene.LoadScene();
            }

            var previousScene = GameController.Instance.PreviousScene;

            if(previousScene != null)
            {
                var previouslyLoadedScenes = GameController.Instance.PreviousScene.connectedScenes;
                foreach(var scene in previouslyLoadedScenes)
                {
                    if(!connectedScenes.Contains(scene) && scene != this)
                    {
                        scene.UnloadScene();
                    }   
                }   

                if(!connectedScenes.Contains(previousScene))
                {
                    previousScene.UnloadScene();
                }
            }



        }
    }

    public void LoadScene()
    {
        if (!IsLoaded)
        {
            sceneNamePopUp = GameObject.Find("SceneName");
            sceneNameText = sceneNamePopUp.GetComponentInChildren<TextMeshProUGUI>();
            sceneNameText.text = gameObject.name;
            sceneNamePopUp.SetActive(false);
            Debug.Log($"Loading {sceneNamePopUp.name}");
            IsLoaded = true;
            SceneManager.LoadSceneAsync(gameObject.name, LoadSceneMode.Additive);
        }
    }

    public void UnloadScene()
    {
        if (IsLoaded)
        {
            SceneManager.UnloadSceneAsync(gameObject.name);
            IsLoaded = false;
        }
    }
    
    public AudioClip SceneMusic => sceneMusic;
}