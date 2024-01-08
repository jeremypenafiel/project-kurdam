using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book : MonoBehaviour, Interactable
{
    public IEnumerator Interact(Transform initiator)
    {
        Debug.Log("Interacting with book");
        yield return null;
    }
}
