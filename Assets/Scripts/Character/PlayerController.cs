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
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        Character.IsMoving = (input.x != 0 || input.y != 0);

        SetPlayerSpeed();
        var targetPos = rb.position + (character.moveSpeed * Time.deltaTime * input);

        if (Character.IsWalkable(targetPos))
        {
            rb.MovePosition(targetPos);
            Character.Animator.IsMoving = Character.IsMoving;
            Character.SetAnimation(input);
        }

        if(!Character.IsMoving && Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log("Interacting");
            StartCoroutine(Interact());
        }
    }

    public void Update()
    {
        while ((encounterList.Count > 0) && (gc.IsInFreeRoamState()))
        {
            GameController.Instance.StartBattle();
            encounterList.Remove(encounterList[0]);
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

 

    public IEnumerator Interact()
    {
        var facingDirection = new Vector3(character.Animator.MoveX, character.Animator.MoveY);
        var interactPosition = rb.position;
        var collider = Physics2D.OverlapCircle(interactPosition,0.5f, GameLayers.I.InteractableLayer); //  increased radius to 0.5f
        if (collider is null)
        {
           Debug.Log("yes");
           yield return collider.GetComponent<Interactable>()?.Interact(transform);
        }
    }

    public Character Character => character;


}
