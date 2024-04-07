using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fader : MonoBehaviour
{
    Image image;


    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void FadeIn(float time)
    {
        Debug.Log("asdijasd");
       image.DOFade(1f, time).WaitForCompletion();
    }
    public void FadeOut(float time)
    {
       image.DOFade(0f, time).WaitForCompletion();
    }



}
