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

            LoadScene();
            GameController.Instance.SetCurrentScene(this);
            switch (sceneNamePopUp.name)
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
            }
            StartCoroutine(PlayPopUpAnimation());
            if (sceneMusic != null)
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
    public IEnumerator PlayPopUpAnimation()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(sceneNamePopUp.transform.DOLocalMoveY(origPos.y - 100f, 0.5f));
        yield return new WaitForSeconds(2.5f);
        sequence.Append(sceneNamePopUp.transform.DOLocalMoveY(origPos.y, 0.5f));
    }



    public AudioClip SceneMusic => sceneMusic;
}