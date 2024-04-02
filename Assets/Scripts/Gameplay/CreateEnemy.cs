using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateEnemy : MonoBehaviour
{

    public GameObject spawner;
    public List<Transform> spawnList;
    public int maxspawn;

    private GameObject enemy;
    private int num =0;
    private Transform spawn;
    private EnemySpawn enemySpawn;
    // Start is called before the first frame update
    void Start()
    {
        
        while (num < maxspawn){
            if (Random.Range(0,100)<50)
            {
                var rnum = Random.Range(0, spawnList.Count);
                spawn = spawnList[rnum];
                enemySpawn= spawn.GetComponent<EnemySpawn>();
                enemy = enemySpawn.enemy;

                gameObject.transform.position = new Vector2(spawn.transform.position.x, spawn.transform.position.y);
                var Xsize = spawn.GetComponent<BoxCollider2D>().size.x / 2;
                var Ysize = spawn.GetComponent<BoxCollider2D>().size.y / 2;
                var xLocation = Random.Range(gameObject.transform.position.x - Xsize, gameObject.transform.position.x + Xsize + 1);
                var yLocation = Random.Range(gameObject.transform.position.y - Ysize, gameObject.transform.position.y + Ysize + 1);

                var randomPosition = new Vector3(xLocation,yLocation, 0);
                Instantiate(enemy, randomPosition, Quaternion.identity);
                num++;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
