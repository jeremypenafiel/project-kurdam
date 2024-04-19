using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLayers : MonoBehaviour
{
    [SerializeField] LayerMask solidObject;
    [SerializeField] LayerMask interactableLayer;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] LayerMask encounterableLayer;
    [SerializeField] LayerMask portalLayer;
    [SerializeField] LayerMask triggersLayer;


    public static GameLayers I { get; set;}

    private void Awake()
    {
        I = this;
    }


    public LayerMask SolidObjectLayer
    {
        get => solidObject;
    }

    public LayerMask InteractableLayer
    {
        get => interactableLayer;
    }

    public LayerMask PlayerLayer
    {
        get => playerLayer;
    }

    public LayerMask EncounterableLayer
    {
        get => encounterableLayer;
    }

    public LayerMask PortalLayer
    {
        get => portalLayer;
    }

    public LayerMask TriggersLayer
    {
        get => triggersLayer;
    }
    public LayerMask TriggerableLayer
    {
        get => portalLayer|encounterableLayer|triggersLayer;
    }

}
