using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

[ExecuteInEditMode]
public class SimpleBlit : MonoBehaviour
{
    public Material TransitionMaterial;
    [SerializeField] public List<Texture> textures;
    
    private float time = 2.75f;
    float newPos = 0;


    private void Awake()
    {
        TransitionMaterial.SetFloat("_Cutoff", 0);
    }

    void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        if (TransitionMaterial != null)
            Graphics.Blit(src, dst, TransitionMaterial);

    }

    public IEnumerator Transition()
    {
        TransitionMaterial.SetTexture("_TransitionTex", textures[0]);

        var rate  = 1 / time;
        var delta = 0.0f;
        while (delta < time) {
            delta += Time.deltaTime;
            newPos = Mathf.Clamp01(delta / time);
            
            TransitionMaterial.SetFloat("_Cutoff", newPos);
            
            yield return null;
        }

       
    }
}
