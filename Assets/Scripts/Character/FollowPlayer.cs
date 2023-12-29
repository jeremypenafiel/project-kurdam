using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] public Transform player;

    private void Update()
    {
        transform.position = player.transform.position + new Vector3(0, 1, -5);
    }
}
