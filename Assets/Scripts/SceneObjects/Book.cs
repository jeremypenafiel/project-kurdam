using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book : MonoBehaviour, Interactable
{
    [SerializeField] Dialog dialog;
    [SerializeField] string line;
    [SerializeField] Sprite closed;
    [SerializeField] Sprite open;

    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = closed;
    }
    public IEnumerator Interact(Transform initiator)
    {
        GetComponent<SpriteRenderer>().sprite = open;
        Debug.Log("Interacting with book");
        yield return DialogManager.Instance.ShowDialog(dialog);
        GetComponent<SpriteRenderer>().sprite = closed;
    }
}
