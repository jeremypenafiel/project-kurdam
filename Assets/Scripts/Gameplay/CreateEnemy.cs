using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateEnemy : MonoBehaviour
{
    public GameObject enemy;
    // Start is called before the first frame update
    void Start()
    {
        for (var i = 0; i < 3; i++)
        {
            Instantiate(enemy, new Vector3(gameObject.transform.position.x-5*i, gameObject.transform.position.y, 0), Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
