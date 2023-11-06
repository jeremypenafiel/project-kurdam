using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public bool isMoving;
    public float distance;
    [SerializeField] float distanceThreshold = 0.5f;

    public LayerMask Encounterable;
    public LayerMask SolidObject;

    public event Action OnEncountered;

    private Vector2 input;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void HandleUpdate()
    {
        if (!isMoving)
        {


            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");



            // remove diagonal movement 
            if (input.x != 0) input.y = 0;

            if (input != Vector2.zero)
            {
                animator.SetFloat("moveX", input.x);
                animator.SetFloat("moveY", input.y);

                var targetPos = transform.position;
                targetPos.x += input.x;
                targetPos.y += input.y;
                if (IsWalkable(targetPos))
                {
                    StartCoroutine(Move(targetPos));
                }
            }
        }
        animator.SetBool("isMoving", isMoving);

    }

    private bool IsWalkable(Vector3 targetPos)
    {
        if (Physics2D.OverlapCircle(targetPos, 0.2f, SolidObject) != null)
        {
            return false;
        }
        return true;
    }
    IEnumerator Move(Vector3 targetPos)
    {
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
    }

    private void CheckForEncounters()
    {
        /*limit using movement or counter*/
        if (Physics2D.OverlapCircle(transform.position, 0.2f, Encounterable) != null)
        {
            if (UnityEngine.Random.Range(1, 101) <= 10)
            {
                animator.SetBool("isMoving", false);
                OnEncountered();
            }
        }

    }
}
