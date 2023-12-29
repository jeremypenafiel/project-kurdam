using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLayers : MonoBehaviour
{
    [SerializeField] LayerMask solidObject;
    [SerializeField] LayerMask interactableLayer;

    public static GameLayers i { get; set;}

    private void Awake()
    {
        i = this;
    }


    public LayerMask SolidObjectLayer
    {
        get => solidObject;
    }

    public LayerMask InteractableLayer
    {
        get => interactableLayer;
    }
}
