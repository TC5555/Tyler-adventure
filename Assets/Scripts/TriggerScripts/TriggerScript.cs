using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TriggerScript : MonoBehaviour
{

    public bool cameraLocked;
    public List<Vector2> points;
    Cinemachine.CinemachineConfiner cinemachineConfiner;
    void OnTriggerEnter2D(Collider2D other)
    {
        Rigidbody2D rigidbody2d = gameObject.GetComponent<Rigidbody2D>();
        rigidbody2d.simulated = false;
        if (cameraLocked)
        {
            cinemachineConfiner = FindObjectOfType<Cinemachine.CinemachineConfiner>();
            ConfinerScript ConfinerShape = GameObject.Find("CameraConfiner").GetComponent<ConfinerScript>();
            cinemachineConfiner.m_Damping = 1;
            ConfinerShape.polygonCollider2D.SetPath(0, points);
             StartCoroutine(SpawnTimeDelay());
            Game game = GameObject.Find("Main Camera").GetComponent<Game>();
            game.canSave = false;


        }
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }

        
            

       
    }
    IEnumerator SpawnTimeDelay()
    {
        yield return new WaitForSeconds(.25f);

        cinemachineConfiner.m_Damping = 0.5f;

        yield return new WaitForSeconds(.25f);

        cinemachineConfiner.m_Damping = 0;
    }

}