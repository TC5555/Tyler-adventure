using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickupScript : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    int healAmount = 2;
    public Sprite altSprite;
    Color32 colorVal = new Color32(255, 255, 255, 255);
    Vector2 direction;
    float fadeAmount = 200;
    float force = 100f;
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        float Rand = Random.Range(0, 100);
        if(Rand >= 50)
        {
            healAmount = 1;
            gameObject.GetComponent<SpriteRenderer>().sprite = altSprite;
        }
        float RandAngle = Random.Range(0, 2*Mathf.PI);

        direction = new Vector2(Mathf.Cos(RandAngle), Mathf.Sin(RandAngle));
        rigidbody2d.AddForce(direction * force);

    }

    // Update is called once per frame
    void Update()
    {
        fadeAmount *= 1.001f;

        if (colorVal.a - (byte)(fadeAmount * Time.deltaTime) < 0)
        {
            Destroy(gameObject);
        }

        colorVal.a -= (byte)(fadeAmount * Time.deltaTime);
        gameObject.GetComponent<Renderer>().material.color = colorVal;
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
        if (p != null&&p.health!=p.maxHealth)
        {
            Debug.Log(p.health + " " + p.maxHealth);
            p.ChangeHealth(healAmount);

            Destroy(gameObject);
        }
    }
    }
