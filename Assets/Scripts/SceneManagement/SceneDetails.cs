using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneDetails : MonoBehaviour
{
    [SerializeField] List<SceneDetails> connectedScenes;
    public bool IsLoaded { get; private  set; } = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Debug.Log($"Entered {gameObject.name}");
            LoadScene();
            GameController.Instance.SetCurrentScene(this);
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
}