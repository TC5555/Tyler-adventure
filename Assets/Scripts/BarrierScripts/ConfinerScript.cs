using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfinerScript : MonoBehaviour
{
    public PolygonCollider2D polygonCollider2D { get; set; }
    public Vector2[] originalPoints { get; set; }
    void Start()
    {
        polygonCollider2D = gameObject.GetComponent<PolygonCollider2D>();
        originalPoints = polygonCollider2D.points;
    }
}
