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
        }
        OnMoveOver?.Invoke();
    }



   public bool IsWalkable(Vector3 targetPos)
    {
        if (Physics2D.OverlapCircle(targetPos, 0.1f, GameLayers.I.SolidObjectLayer| GameLayers.I.InteractableLayer) != null)
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

        var thingy = BoxCast(transform.position + direction, new Vector2(0.2f, 0.2f), 0f, direction, difference.magnitude - 1, GameLayers.I.SolidObjectLayer | GameLayers.I.InteractableLayer | GameLayers.I.PlayerLayer);
        Debug.Log(thingy.collider);
        Debug.Log(transform.position);
        Debug.Log(targetPos);
        if (Physics2D.BoxCast(transform.position + direction, new Vector2(0.2f, 0.2f), 0f, direction, difference.magnitude - 1, GameLayers.I.SolidObjectLayer | GameLayers.I.InteractableLayer | GameLayers.I.PlayerLayer) == true)
        {
            Debug.Log("something ion the way");
            return false;
        };
        Debug.Log("path clear");
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

    static public RaycastHit2D BoxCast(Vector2 origen, Vector2 size, float angle, Vector2 direction, float distance, int mask)
    {
        RaycastHit2D hit = Physics2D.BoxCast(origen, size, angle, direction, distance, mask);
        //Setting up the points to draw the cast
        Vector2 p1, p2, p3, p4, p5, p6, p7, p8;
        float w = size.x * 0.5f;
        float h = size.y * 0.5f;
        p1 = new Vector2(-w, h);
        p2 = new Vector2(w, h);
        p3 = new Vector2(w, -h);
        p4 = new Vector2(-w, -h);

        Quaternion q = Quaternion.AngleAxis(angle, new Vector3(0, 0, 1));
        p1 = q * p1;
        p2 = q * p2;
        p3 = q * p3;
        p4 = q * p4;

        p1 += origen;
        p2 += origen;
        p3 += origen;
        p4 += origen;

        Vector2 realDistance = direction.normalized * distance;
        p5 = p1 + realDistance;
        p6 = p2 + realDistance;
        p7 = p3 + realDistance;
        p8 = p4 + realDistance;


        //Drawing the cast
        Color castColor = hit ? Color.red : Color.green;
        Debug.DrawLine(p1, p2, castColor);
        Debug.DrawLine(p2, p3, castColor);
        Debug.DrawLine(p3, p4, castColor);
        Debug.DrawLine(p4, p1, castColor);

        Debug.DrawLine(p5, p6, castColor);
        Debug.DrawLine(p6, p7, castColor);
        Debug.DrawLine(p7, p8, castColor);
        Debug.DrawLine(p8, p5, castColor);

        Debug.DrawLine(p1, p5, Color.grey);
        Debug.DrawLine(p2, p6, Color.grey);
        Debug.DrawLine(p3, p7, Color.grey);
        Debug.DrawLine(p4, p8, Color.grey);
        if (hit)
        {
            Debug.DrawLine(hit.point, hit.point + hit.normal.normalized * 0.2f, Color.yellow);
        }

        return hit;
    }

    public CharacterAnimator Animator { get => animator; }
}
