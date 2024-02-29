using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float distance;


    private Vector2 input;

    private Character character;
    private Vector3 offset;
    public static PlayerController i { get; private set; }

    private void Awake()
    {
        character = GetComponent<Character>();
        offset = new Vector3(0, Character.offsetY);
        i = this;
    }

    public void HandleUpdate()
    {
        /*if(Input.GetKeyUp(KeyCode.Escape))
        {
            PauseScreen();
        }*/
        if (!character.IsMoving)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            // remove diagonal movement 
             //if (input.x != 0) input.y = 0;                                                                               //the movement overhaul lol

            if (input != Vector2.zero)
            {
                /*var colliders = (Physics2D.OverlapCircleAll(transform.position, 0.2f, GameLayers.I.TriggerableLayer));
                foreach (var collider in colliders)
                {
                    var triggerable = collider.GetComponent<IPLayerTriggerable>();
                    if (triggerable != null)
                    {
                        triggerable.OnPlayerTriggered(this);
                        break;
                    }
                }*/
                if (Character.IsWalkable(transform.position)==true){ 
                StartCoroutine(character.Move(input, OnMoveOver));
                }
            }
        }

        character.HandleUpdate();

        if (Input.GetKeyDown(KeyCode.Z))
        {

            StartCoroutine(Interact());
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

    private void OnMoveOver()
    {
        var colliders = (Physics2D.OverlapCircleAll(transform.position - offset, 0.2f,  GameLayers.I.TriggerableLayer));
        foreach (var collider in colliders)
        {
            var triggerable = collider.GetComponent<IPLayerTriggerable>();
            if (triggerable != null)
            {
                
                triggerable.OnPlayerTriggered(this);
                break;
            }
        }
    }

 

    IEnumerator Interact()
    {
        var facingDirection = new Vector3(character.Animator.MoveX, character.Animator.MoveY);
        var interactPosition = transform.position + facingDirection;
        var collider = Physics2D.OverlapCircle(interactPosition, 0.1f, GameLayers.I.InteractableLayer);
        if (collider != null)
        {

           yield return collider.GetComponent<Interactable>()?.Interact(transform);
        }
    }

    public Character Character => character;
}
