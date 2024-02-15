using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Prologue : MonoBehaviour
{
    Fader fader;

    // Start is called before the first frame update
    void Start()
    {
       StartCoroutine(FinishPrologue());
       fader = FindObjectOfType<Fader>();
    }

    IEnumerator FinishPrologue()
    {

        yield return new WaitUntil(()=>Input.GetKeyDown(KeyCode.Z));
        fader.FadeIn(0.5f);
        yield return SceneManager.LoadSceneAsync(2);
        fader.FadeOut(0.5f);    
    }
    
}
