using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerScript : MonoBehaviour
{

    public float spawnTime;        // The amount of time between each spawn.
    public float spawnDelay;       // The amount of time before spawning starts.
    public GameObject enemy;

    public int amountSpawned;


    void Awake()
    {
        gameObject.SetActive(false);
    }

    void Start()
    {
      
            StartCoroutine(SpawnTimeDelay());
       
    }

    IEnumerator SpawnTimeDelay()
    {
        yield return new WaitForSeconds(spawnDelay);
    
            for (int i = amountSpawned; i > 0; i--)
            {
                GameObject e = Instantiate(enemy, transform.position, Quaternion.identity);
                e.tag = "SpawnedEnemy";
                yield return new WaitForSeconds(spawnTime);
            }
            Destroy(gameObject);
       

        }
}