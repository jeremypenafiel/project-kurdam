﻿using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

[ExecuteInEditMode]
public class SimpleBlit : MonoBehaviour
{
    public Material TransitionMaterial;
    [SerializeField] public List<Texture> textures;
    [SerializeField] private float transitionDuration = 2.75f;
    [SerializeField] private int transitionNumber;
    private void Awake()
    {
        TransitionMaterial.SetFloat("_Cutoff", 0);
    }

    void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        if (TransitionMaterial != null)
            Graphics.Blit(src, dst, TransitionMaterial);
    }

    public IEnumerator TransitionIn()
    {
        // Sets Transition Texture or Pattern
        TransitionMaterial.SetTexture("_TransitionTex", textures[transitionNumber]);
        
        // Does transition as long as duration
        var delta = 0.0f;
        float newPos;
        while (delta < transitionDuration) {
            delta += Time.deltaTime;
            newPos = Mathf.Clamp01(delta / transitionDuration);
            
            // Increases Cutoff property gradually, thereby doing the transition
            TransitionMaterial.SetFloat("_Cutoff", newPos);
            
            yield return null;
        }

       
    }
}
