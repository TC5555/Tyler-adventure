using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    public Sprite altSprite;
    public Vector2 direction;
    float force = 100f;
    public string WeaponAdded;
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
   
        float RandAngle = Random.Range(0, 2 * Mathf.PI);

        direction = new Vector2(Mathf.Cos(RandAngle), Mathf.Sin(RandAngle));
        rigidbody2d.AddForce(direction * force);

    }

    void Update()
    {
        if (rigidbody2d.velocity.magnitude < 0.5f)
        {
            rigidbody2d.velocity *= 0;
        }
        else
        {
            rigidbody2d.AddForce(direction * -force * Time.deltaTime);
        }
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        PlayerScript p = other.collider.GetComponent<PlayerScript>();
        if (p != null)
        {
            p.Weapons.Add(WeaponAdded);
        }
    }
}
