using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneDetails : MonoBehaviour
{
    [SerializeField] List<SceneDetails> connectedScenes;
    [SerializeField] AudioClip sceneMusic;



    public bool IsLoaded { get; private  set; } = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Debug.Log($"Entered {gameObject.name}");

            LoadScene();
            GameController.Instance.SetCurrentScene(this);

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