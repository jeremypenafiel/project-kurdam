using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public static PlayerController i { get; private set; }
    public bool IsRunning { get => isRunning; }
    public bool IsSneaking { get => isSneaking; }

    public Rigidbody2D rb;

    public List<Aswang> encounterList;
    GameController gc;

    private void Awake()
    {
        character = GetComponent<Character>();
        rb = GetComponent<Rigidbody2D>();
        gc = GameObject.Find("GameController").GetComponent<GameController>();
        i = this;
    }


    public void HandleUpdate()
    { var position = rb.position;
       GetInput();
       
       // Encounter logic from Jorge
       while ((encounterList.Count > 0) && (gc.IsInFreeRoamState()))
       {
           input.x = 0; // set input to 0 to stop player movement
           input.y = 0;
           gc.StartBattle();
           encounterList.Remove(encounterList[0]);
       }
       
       // This block is for interacting with interactable objects
       if (!Character.IsMoving && Input.GetKeyDown(KeyCode.Z))
       {
           Debug.Log("Interacting");
           StartCoroutine(Interact(position));
       }
       
       // This is for teleporting to other locations
       Teleport(position);
    }

    public void GetInput()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {
        // For movement
        Character.IsMoving = (input.x != 0 || input.y != 0);
        SetPlayerSpeed();
        var targetPos = rb.position + (character.moveSpeed * Time.deltaTime * input);
        Character.Animator.IsMoving = Character.IsMoving;
        if (Character.IsWalkable(targetPos))
        {
            rb.MovePosition(targetPos);
            Character.SetAnimation(input);
        }

       
    }


    private void SetPlayerSpeed()
    {
        isRunning = Input.GetKey(KeyCode.X);
        isSneaking = Input.GetKey(KeyCode.C);

        if (isRunning)
        {
            character.moveSpeed = runSpeed;
            character.Animator.IsRunning = true;
        }
        else if (isSneaking)
        {
            character.moveSpeed = sneakSpeed;
            character.Animator.IsSneaking = true;
        }
        else
        {
            character.moveSpeed = walkSpeed;
            character.Animator.IsSneaking = false;
            character.Animator.IsRunning = false;
        }
    }

 

    public IEnumerator Interact(Vector2 position)
    {
        var collider = Physics2D.OverlapCircle(position,0.5f, GameLayers.I.InteractableLayer); //  increased radius to 0.5f
        if (collider is not null)
        {
           Debug.Log("yes");
           yield return collider.GetComponent<Interactable>()?.Interact(transform);
        }
    }

    public void Teleport(Vector2 position)
    {
        var collision = Physics2D.OverlapCircle(position,0.5f, GameLayers.I.PortalLayer); //  increased radius to 0.5f
        if (collision is null) return;
        Debug.Log("yes");
        collision.GetComponent<IPLayerTriggerable>()?.OnPlayerTriggered(this);
    }

    public Character Character => character;


}
