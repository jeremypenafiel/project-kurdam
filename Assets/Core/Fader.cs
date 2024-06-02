using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fader : MonoBehaviour
{
    Image image;
    public Tweener tweener;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public IEnumerator FadeIn(float time)
    {
        Debug.Log("FADER CALLED");
        tweener = image.DOFade(1f, time);
        yield return tweener.WaitForCompletion();
    }
    public IEnumerator FadeOut(float time)
    {
        Debug.Log("nagrun FADE OUT");
        tweener = image.DOFade(0f, time);
        yield return tweener.WaitForCompletion();
    }



}
