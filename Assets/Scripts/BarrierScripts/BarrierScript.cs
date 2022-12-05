using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierScript : MonoBehaviour
{
    void Awake()
    {
        gameObject.SetActive(false);
    }
    void Update()
    {
        if (GameObject.FindGameObjectWithTag("SpawnedEnemy")==null&&GameObject.FindGameObjectWithTag("Spawner") == null)
        {
            Destroy(gameObject);
        }
    }

}
