using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerScript : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
         foreach (Transform child in transform)
         {              
                 child.gameObject.SetActive(true);
         }

        Rigidbody2D rigidbody2d = gameObject.GetComponent<Rigidbody2D>();
        rigidbody2d.simulated = false;
    }
  
}
