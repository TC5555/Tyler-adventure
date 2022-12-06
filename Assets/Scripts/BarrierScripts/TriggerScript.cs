using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TriggerScript : MonoBehaviour
{
    public bool cameraLocked;
    public Vector2[] points;
    Cinemachine.CinemachineConfiner cinemachineConfiner;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (cameraLocked)
        {
            cinemachineConfiner = FindObjectOfType<Cinemachine.CinemachineConfiner>();
            ConfinerScript ConfinerShape = GameObject.Find("CameraConfiner").GetComponent<ConfinerScript>();
            cinemachineConfiner.m_Damping = 1;
            ConfinerShape.polygonCollider2D.SetPath(0, points);
             StartCoroutine(SpawnTimeDelay());

        }
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }

        Rigidbody2D rigidbody2d = gameObject.GetComponent<Rigidbody2D>();
        rigidbody2d.simulated = false;
    }
    IEnumerator SpawnTimeDelay()
    {
        yield return new WaitForSeconds(.5f);

        cinemachineConfiner.m_Damping = 0;


    }

}