using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NPCController : MonoBehaviour, Interactable
{
    [SerializeField] Dialog dialog;
    [SerializeField] List<Vector2> movementPattern;
    [SerializeField] float timeBetweenPattern;

    float idleTimer;
    NPCState state;
    int currentPattern = 0;

    Character character;

    private void Awake()
    {
        character = GetComponent<Character>();
    }
    public IEnumerator Interact(Transform initiator)
    {
        if (state == NPCState.Idle)
        {
            state = NPCState.Dialog;
            character.LookTowards(initiator.position);
            yield return DialogManager.Instance.ShowDialog(dialog);

            idleTimer = 0f;
            state = NPCState.Idle;
        }
    }

    private void Update()
    {

        if (state == NPCState.Idle)
        {
            idleTimer += Time.deltaTime;
            if (idleTimer > timeBetweenPattern)
            {
                idleTimer = 0f;
                if(movementPattern.Count > 0)
                {
                    StartCoroutine(Walk());
                }
                
            }
        }
        character.HandleUpdate();

    }   

    private IEnumerator Walk()
    {
        state = NPCState.Walking;

        var oldPosition = transform.position;

        yield return character.Move(movementPattern[currentPattern]);

        if (transform.position != oldPosition)
            currentPattern = (currentPattern + 1) % movementPattern.Count;


        state = NPCState.Idle;

    }

    public enum NPCState { Idle, Walking, Dialog }
}
