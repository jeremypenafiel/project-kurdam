using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public class StartMenu : MonoBehaviour
{
    public void Update()
    {
        StartCoroutine(HandleUpdate());
    }
    IEnumerator HandleUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            yield return SceneManager.LoadSceneAsync(2);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            

            Application.Quit();
        }

    }


}
