using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;

public class Character : MonoBehaviour
{
    CharacterAnimator animator;
    public float moveSpeed;
    public float distance;
    public float distanceThreshold = 0.5f;
    public bool IsMoving { get; private set; }


    private void Awake()
    {
        animator = GetComponent<CharacterAnimator>();
    
    }

    public IEnumerator Move(Vector3 moveVector, Action OnMoveOver = null)
    {

        animator.MoveX = moveVector.x;
        animator.MoveY = moveVector.y;

        var targetPos = transform.position;
        targetPos.x += moveVector.x;
        targetPos.y += moveVector.y;

        if(!IsWalkable(targetPos))
        {
            yield break;
        }

        
        IsMoving = true;
        distance = 0;
        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            distance += moveSpeed * Time.deltaTime;
            yield return null;
        }
        transform.position = targetPos;
        IsMoving = false;
        if (distance >= distanceThreshold)
        {
            distance = 0;
            OnMoveOver?.Invoke();
        }
    }


    private bool IsWalkable(Vector3 targetPos)
    {
        if (Physics2D.OverlapCircle(targetPos, 0.1f, GameLayers.i.SolidObjectLayer| GameLayers.i.InteractableLayer) != null)
        {
            return false;
        }
        return true;
    }

    public void HandleUpdate()
    {
        animator.IsMoving = IsMoving;
    }

    public CharacterAnimator Animator { get => animator; }
}
