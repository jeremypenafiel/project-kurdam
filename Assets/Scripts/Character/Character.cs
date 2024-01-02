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
    public float offsetY = 0.3f;
    public bool IsMoving { get; private set; }

    
    private void Awake()
    {
        animator = GetComponent<CharacterAnimator>();
        SetPositionAndSnapToTile(transform.position);
    
    }

    public IEnumerator Move(Vector3 moveVector, Action OnMoveOver = null)
    {

        animator.MoveX = Mathf.Clamp(moveVector.x, -1f, 1f);
        animator.MoveY = Mathf.Clamp(moveVector.y, -1f, 1f);

        var targetPos = transform.position;
        targetPos.x += moveVector.x;
        targetPos.y += moveVector.y;

        if(!IsPathClear(targetPos))
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



   public bool IsWalkable(Vector3 targetPos)
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

    private bool IsPathClear(Vector3 targetPos)
    {
        var difference = targetPos - transform.position;
        var direction = difference.normalized;

        if (Physics2D.BoxCast(transform.position + direction, new Vector2(0.2f, 0.2f), 0f, direction, difference.magnitude - 1, GameLayers.i.SolidObjectLayer | GameLayers.i.InteractableLayer | GameLayers.i.PlayerLayer) == true)
        {
            return false;
        };
        return true;
    }

    public void LookTowards(Vector3 targetPos)
    {
        var xDifference = Mathf.Floor(targetPos.x - transform.position.x);
        var yDifference = Mathf.Floor(targetPos.y - transform.position.y);

        if(xDifference == 0 || yDifference == 0)
        {
            animator.MoveX = Mathf.Clamp(xDifference, -1f, 1f);
            animator.MoveY = Mathf.Clamp(yDifference, -1f, 1f);
        }
        else
        {
            Debug.LogError("Character is not aligned with the grid");
        }

    }

    public void SetPositionAndSnapToTile(Vector2 pos)
    {
        pos.x = Mathf.Floor(pos.x) + 0.5f;
        pos.y = Mathf.Floor(pos.y) + 0.8f;
        transform.position = pos;
    }

    public CharacterAnimator Animator { get => animator; }
}
