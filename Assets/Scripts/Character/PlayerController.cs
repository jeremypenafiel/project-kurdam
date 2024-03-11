using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float distance;


    private Vector2 input;
    private const float walkSpeed = 5.0f;

    private const float runSpeed = 7.5f;

    private const float sneakSpeed = 3.0f;

    private bool isRunning =  false;

    private bool isSneaking =  false;

    private Character character;
    private Vector3 offset;
    public static PlayerController i { get; private set; }
    public bool IsRunning { get => isRunning; }
    public bool IsSneaking { get => isSneaking; }

    public Rigidbody2D rb;

    private void Awake()
    {
        character = GetComponent<Character>();
        offset = new Vector3(0, Character.offsetY);
        rb = GetComponent<Rigidbody2D>();

        i = this;
    }

    private void FixedUpdate()
    {
        HandleUpdate();
    }


    public void HandleUpdate()
    {

        if (!character.IsMoving)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            //Debug.Log(input.x);

            SetPlayerSpeed();
            var targetPos = rb.position + (character.moveSpeed * Time.deltaTime * input);

            if (Character.IsWalkable(targetPos))
            {
                rb.MovePosition(targetPos);
                Character.IsMoving = true;
                Character.SetAnimation(input);

                if (rb.position == targetPos)
                {
                    Character.IsMoving = false;
                    Debug.Log("not moving");
                }


            }




            // remove diagonal movement 
            //if (input.x != 0) input.y = 0;                                                                               //the movement overhaul lol

            /* if (input != Vector2.zero)
             {

                 if (Character.IsWalkable(transform.position)==true){ 

                     StartCoroutine(character.Move(input, OnMoveOver));
                 }
        }*/
        }

      /*  character.HandleUpdate();

        if (Input.GetKeyDown(KeyCode.Z) && !character.IsMoving)
        {

            StartCoroutine(Interact());
        }*/

    }


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

    private void SetPlayerSpeed()
    {
        isRunning = Input.GetKey(KeyCode.X);
        isSneaking = Input.GetKey(KeyCode.C);

        if (isRunning)
        {
            character.moveSpeed = runSpeed;
        }
        else if (isSneaking)
        {
            character.moveSpeed = sneakSpeed;
        }
        else
        {
            character.moveSpeed = walkSpeed;
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
