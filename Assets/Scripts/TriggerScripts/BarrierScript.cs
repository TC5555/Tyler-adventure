using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierScript : MonoBehaviour
{
    public bool confined;
    void Awake()
    {
        gameObject.SetActive(false);
    }
    void Update()
    {
        if (GameObject.FindGameObjectWithTag("SpawnedEnemy")==null&&GameObject.FindGameObjectWithTag("Spawner") == null)
        {
            if (confined)
            {
                ConfinerScript Confiner = GameObject.Find("CameraConfiner").GetComponent<ConfinerScript>();
               
                Confiner.polygonCollider2D.SetPath(0,Confiner.originalPoints);

                    Game game = GameObject.Find("Main Camera").GetComponent<Game>();
                 game.canSave = true;
                

            }
            Destroy(gameObject);
        }
    }

}
