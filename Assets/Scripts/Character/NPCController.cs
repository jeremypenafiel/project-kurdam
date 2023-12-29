using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NPCController : MonoBehaviour, Interactable
{
    [SerializeField] Dialog dialog;

    Character character;

    private void Awake()
    {
        character = GetComponent<Character>();
    }
    public void Interact()
    {
        StartCoroutine(DialogManager.Instance.ShowDialog(dialog));
    }

    private void Update()
    {
        character.HandleUpdate();
    }   
}
