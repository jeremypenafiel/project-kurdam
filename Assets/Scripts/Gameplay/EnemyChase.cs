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
    public float musicCheckDistance = 15f;

    public bool shouldRotate;
    public LayerMask chase;

    private Transform target;
    private Rigidbody2D rb;
    private Animator anim;
    private Vector2 movement;
    public Vector3 dir;
    private Vector2 look;

    private bool isInChaseRange;
    private bool isInEncounterRange;
    private bool canMove;
    private GameObject player;

    private bool isCreepyMusicPlaying = false;

    private Character character;
    
    private void Start()
    {
        currentCheckRadius = checkRadiusDefault;
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindWithTag("Player").transform;
        player = GameObject.Find("Player");
        character = GetComponent<Character>();
        if (character != null)
        {
            Debug.Log($"{character} is not null");
            Debug.Log(character.Animator);
            
        }



    }

    private void Update()
    {
        SetCheckRadius();
        var targetPos = target.position;
        var unitPos = transform.position;

        isInChaseRange = Physics2D.OverlapCircle(unitPos, currentCheckRadius, chase);
        isInEncounterRange = Physics2D.OverlapCircle(unitPos, encounterRadius, chase);
        canMove = GameController.Instance.IsInFreeRoamState();
        dir = targetPos - unitPos;
        if (!isCreepyMusicPlaying)
        {
            CheckIfPlayCreepyMusic(targetPos, unitPos);
        }
        else
        {
            SetAmbientVolume(targetPos, unitPos);
            CheckIfNotPlayCreepyMusic(targetPos, unitPos);
        }

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        dir.Normalize();
        
        movement = dir;

/*        if (shouldRotate)
        {
            anim.SetFloat("x", dir.x);
            anim.SetFloat("y", dir.y);
        }*/
    }
    private void SetAmbientVolume(Vector3 targetPos, Vector3 unitPos)
    {
        var dist = Vector3.Distance(targetPos, unitPos);
        var clampedDistance = Mathf.Clamp01((musicCheckDistance - dist)/ musicCheckDistance);
        AudioManager.i.SetAmbientVolume(clampedDistance);
    }
    private void CheckIfPlayCreepyMusic(Vector3 targetPos, Vector3 unitPos)
    {
        var dist = Vector3.Distance(targetPos, unitPos);
        if (dist < musicCheckDistance)
        {
            isCreepyMusicPlaying = true;
            AudioManager.i.PlayAmbientSound(null, true);
        }
    }

    private void CheckIfNotPlayCreepyMusic(Vector3 targetPos, Vector3 unitPos)
    {
        var dist = Vector3.Distance(targetPos, unitPos);
        if (dist >= musicCheckDistance)
        {
            isCreepyMusicPlaying = false;
            AudioManager.i.StopPlayAmbientSound();
        }
    }

    private void FixedUpdate()
    {
        
        if (isInChaseRange && !isInEncounterRange && canMove)
        {
            look.x = movement.x;

            MoveCharacter(movement);
            character.SetAnimation(look);
            
        }
        
        if (isInEncounterRange) 
        {
            rb.velocity = Vector2.zero;
            gameObject.name = "Encounter";
            var aswang = gameObject.GetComponent<MapArea>().GetRandomWildAswang();
            Debug.Log(aswang.Base);
            var list = player.GetComponent<PlayerController>();
            list.encounterList.Add(new Aswang(aswang.Base, aswang.Level));
            Destroy(gameObject);
        }

        


    }

    private void MoveCharacter(Vector2 dir)
    {
        character.Animator.IsMoving = true;
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
