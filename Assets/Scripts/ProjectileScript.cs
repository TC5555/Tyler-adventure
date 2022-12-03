using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    public int damageAmount;
    public bool spread;
    public float force;
    public int spreadRange;
    public int distance;
    bool active = false;
    Vector2 origin;


    void Awake()
    {
      
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    public void Launch(Vector2 direction, Vector2 origin)
    {
        this.origin = origin;
        active = true;
        if (spread)
        {
            float spreadRand = Random.Range(-spreadRange * Mathf.PI / 360f, spreadRange * Mathf.PI / 360f);

            float directionAngle = Mathf.Atan2(direction.y, direction.x);

            transform.Rotate(new Vector3(0, 0, Mathf.Atan2(direction.y, direction.x) * (180f / Mathf.PI) + Mathf.Atan2(Mathf.Sin(directionAngle + spreadRand), Mathf.Cos(directionAngle + spreadRand)) * (180f / Mathf.PI)));
          
            direction.x = Mathf.Cos(directionAngle + spreadRand);
            direction.y = Mathf.Sin(directionAngle + spreadRand);

        }
        rigidbody2d.AddForce(direction * force);
    }

    void Update()
    {
        
        if (!active)
        {
            return;
        }

        if (transform.position.x > origin.x + distance|| transform.position.x < origin.x - distance || transform.position.y > origin.y + distance || transform.position.y < origin.y - distance)
        {

            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {

        if (!active)
        {
            return;
        }
        PlayerScript p = other.collider.GetComponent<PlayerScript>();
        if (p != null)
        {
            p.ChangeHealth(damageAmount);

            Destroy(gameObject);
        }
        
        EnemyScript e = other.collider.GetComponent<EnemyScript>();
        if (e != null)
        {
            e.Damage(damageAmount);

            Destroy(gameObject);
        }

        Destroy(gameObject);
    }
}