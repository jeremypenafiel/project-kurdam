using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    public float speed;
    public float checkRadiusDefault;
    public float encounterRadius;
    private float currentCheckRadius;

    public bool shouldRotate;
    public LayerMask chase;

    private Transform target;
    private Rigidbody2D rb;
    private Animator anim;
    private Vector2 movement;
    public Vector3 dir;

    private bool isInChaseRange;
    private bool isInEncounterRange;
    private bool canMove;

    
    private void Start()
    {
        currentCheckRadius = checkRadiusDefault;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        target = GameObject.FindWithTag("Player").transform;

    }

    private void Update()
    {
        SetCheckRadius();

        anim.SetBool("isRunning", isInChaseRange);
        isInChaseRange = Physics2D.OverlapCircle(transform.position, currentCheckRadius, chase);
        isInEncounterRange = Physics2D.OverlapCircle(transform.position, encounterRadius, chase);
        canMove = GameController.Instance.IsInFreeRoamState();
        dir = target.position - transform.position; float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        dir.Normalize();
        movement = dir;

        if (shouldRotate)
        {
            anim.SetFloat("x", dir.x);
            anim.SetFloat("y", dir.y);
        }
    }

    private void FixedUpdate()
    {
        

        if (isInChaseRange && !isInEncounterRange && canMove)
        {
            MoveCharacter(movement);
        }
        if (isInEncounterRange)
        {
            rb.velocity = Vector2.zero;
            gameObject.name = "Encounter";
            GameController.Instance.StartBattle();
            Destroy(gameObject);
        }
    }

    private void MoveCharacter(Vector2 dir)
    {
        rb.MovePosition((Vector2)transform.position + (dir * speed * Time.deltaTime));

    }

    private void SetCheckRadius()
    {
        if (PlayerController.i.IsSneaking)
        {
            currentCheckRadius = 0.5f * checkRadiusDefault;
        }
        else
        {
            currentCheckRadius = checkRadiusDefault;
        }


        if (PlayerController.i.IsRunning)
        {
            currentCheckRadius = 1.1f * checkRadiusDefault;
        }
        else
        {
            currentCheckRadius = checkRadiusDefault;
        }
    }
}
