using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerScript : MonoBehaviour
{
    public bool cameraLocked;
    public Vector2[] points;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (cameraLocked)
        {
            ConfinerScript Confiner = GameObject.Find("CameraConfiner").GetComponent<ConfinerScript>();

            Confiner.polygonCollider2D.SetPath(0, points);

        }
        foreach (Transform child in transform)
         {              
                 child.gameObject.SetActive(true);
         }

        Rigidbody2D rigidbody2d = gameObject.GetComponent<Rigidbody2D>();
        rigidbody2d.simulated = false;
    }
  
}
