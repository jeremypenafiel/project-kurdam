using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour, IPLayerTriggerable
{
    public void OnPlayerTriggered(PlayerController player)
    {
        Debug.Log("Yup");
    }
}
