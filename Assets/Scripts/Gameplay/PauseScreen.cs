using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{

    void Update()
    {
        StartCoroutine(HandleInput());
    }

    IEnumerator HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            GameController.Instance.PauseGame();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            yield return UnityEditor.EditorApplication.isPlaying = false;
        }
    }
}
