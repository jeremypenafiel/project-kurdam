using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour, ISavable
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

    private bool isMoveOneTile = false;
    private Vector2 moveDirection;

    public Rigidbody2D rb;

    public List<Aswang> encounterList;
    public GameController gc;
    public Player player;

    IPLayerTriggerable currentlyInTrigger=null;

    private void Awake()
    {
        character = GetComponent<Character>();
        rb = GetComponent<Rigidbody2D>();
        gc = GameObject.Find("GameController").GetComponent<GameController>();
        i = this;
        player = gameObject.GetComponent<Player>();
        StoryItem.OnQuestIncomplete += (Vector2 direction) =>
        {
            isMoveOneTile = true;
            moveDirection = direction;
        };
    }



    public void HandleUpdate()
    {
        var position = rb.position;
       GetInput();
       
       while ((encounterList.Count > 0) && (gc.IsInFreeRoamState()))
       {
           input.x = 0;
           input.y = 0;
           gc.StartBattle();
           encounterList.Remove(encounterList[0]);
       }
       
       if (!Character.IsMoving && Input.GetKeyDown(KeyCode.Z))
       {
           Debug.Log("Interacting");
           StartCoroutine(Interact(position, this));
       }
       
        Teleport(position);
        StoryTrigger(position);
    }

    public void GetInput()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {

        Character.IsMoving = (input.x != 0 || input.y != 0);
        
        SetPlayerSpeed();
        
        var targetPos = rb.position + (character.moveSpeed * Time.deltaTime * Vector2.ClampMagnitude(input, 1)); // Vector2.ClampMagnitude is used to prevent diagonal movement from being faster than horizontal or vertical movement
        Character.Animator.IsMoving = Character.IsMoving;
        if (Character.IsWalkable(targetPos))
        {
            rb.MovePosition(targetPos);
            Character.SetAnimation(input);
        }
        
        // if (isMoveOneTile)
        // {
        //     MoveOneTile(moveDirection);
        //     isMoveOneTile = false;
        // }

       
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

 

    public IEnumerator Interact(Vector2 position, PlayerController player)
    {
        var collider = Physics2D.OverlapCircle(position,0.5f, GameLayers.I.InteractableLayer); //  increased radius to 0.5f
        if (collider is not null)
        {
           Debug.Log("yes");
           yield return collider.GetComponent<Interactable>()?.Interact(transform, player);
        }
    }

    public void Teleport(Vector2 position)
    {
        var collision = Physics2D.OverlapCircle(position,0.5f, GameLayers.I.PortalLayer); //  increased radius to 0.5f
        if (collision is null) return;
        Debug.Log("yes");
        collision.GetComponent<IPLayerTriggerable>()?.OnPlayerTriggered(this);
    }

    public void StoryTrigger(Vector2 position)
    {
        IPLayerTriggerable triggerable;
        var collision = Physics2D.OverlapCircle(position, 0.5f, GameLayers.I.TriggersLayer); //  increased radius to 0.5f

        if (collision is null)
        {
            currentlyInTrigger = null;
            return;
        }
        triggerable = collision.GetComponent<IPLayerTriggerable>();
        if (triggerable == currentlyInTrigger && !triggerable.TriggerRepeatedly) return;
        input.x = 0;
        input.y = 0;
        currentlyInTrigger = triggerable;
        triggerable.OnPlayerTriggered(i);

    }

    public object CaptureState()
    {
        float[] position = new float[] { transform.position.x,  transform.position.y };
        return position;
    }

    public void RestoreState(object state)
    {
        var position = (float[])state;
        transform.position = new Vector3(position[0], position[1]);
    }

    public Character Character => character;


}
