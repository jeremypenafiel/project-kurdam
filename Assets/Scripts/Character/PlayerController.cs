using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    

    public float distance;
    /*[SerializeField] float distanceThreshold = 0.5f;*/

    public LayerMask Encounterable;
    public LayerMask portallayer;

    public LayerMask PortalLayer
    
    {
        get => portallayer; /*set=>PortalLayer = value;*/
    }
    public static PlayerController i { get; set; }
    public LayerMask TriggerableLayers
    {
        get => portallayer;
    }

    public event Action OnEncountered;

    private Vector2 input;

    private Character character;

    private void Awake()
    {
        character = GetComponent<Character>();

        i = this;
    }


    public void HandleUpdate()
    {
        if (!character.IsMoving)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            // remove diagonal movement 
            if (input.x != 0) input.y = 0;

            if (input != Vector2.zero)
            {
                var colliders = (Physics2D.OverlapCircleAll(transform.position, 0.2f, PlayerController.i.TriggerableLayers));
                foreach (var collider in colliders)
                {
                    var triggerable = collider.GetComponent<IPLayerTriggerable>();
                    if (triggerable != null)
                    {
                        triggerable.OnPlayerTriggered(this);
                        break;
                    }
                }
                StartCoroutine(character.Move(input, CheckForEncounters));
            }
        }

        character.HandleUpdate();

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Interact();
        }

    }

   /* private bool IsWalkable(Vector3 targetPos)
    {
        if (Physics2D.OverlapCircle(targetPos, 0.1f, SolidObject | interactableLayer) != null)
        {
            return false;
        }
        return true;
    }
    IEnumerator Move(Vector3 targetPos)
    {
        var colliders =(Physics2D.OverlapCircleAll(transform.position, 0.2f, PlayerController.i.TriggerableLayers));
        foreach (var collider in colliders)
        {
            var triggerable =collider.GetComponent < IPLayerTriggerable >();
            if (triggerable != null)
            {
                triggerable.OnPlayerTriggered(this);
                break;
            }
        }
        isMoving = true;
        distance = 0;
        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            distance += moveSpeed * Time.deltaTime;
            yield return null;
        }
        transform.position = targetPos;
        isMoving = false;
        if (distance >= distanceThreshold)
        {
            distance = 0;
            CheckForEncounters();
        }
    }*/

    private void CheckForEncounters()
    {
        /*limit using movement or counter*/
        if (Physics2D.OverlapCircle(transform.position, 0.2f, Encounterable) != null)
        {
            if (UnityEngine.Random.Range(1, 101) <= 10)
            {
                character.Animator.IsMoving = false;
                OnEncountered();
            }
        }

    }

    void Interact()
    {
        var facingDirection = new Vector3(character.Animator.MoveX, character.Animator.MoveY);
        var interactPosition = transform.position + facingDirection;
        var collider = Physics2D.OverlapCircle(interactPosition, 0.1f, GameLayers.i.InteractableLayer);
        if (collider != null)
        {
            collider.GetComponent<Interactable>()?.Interact(transform);
        }
    }

    public Character Character => character;
}
