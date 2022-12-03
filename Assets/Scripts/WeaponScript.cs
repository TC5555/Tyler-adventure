using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    Vector2 direction;
    public GameObject ProjectilePrefab;
    Vector2 position;
    bool canShoot;
    float timer;
    public float shootTime;
    bool lookHeld;
    public int shootMulti;
    Rigidbody2D rigidbody2d;
    private void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>(); 
    }

    void Update()
    {
        
        if (!canShoot)
        {
            timer -= Time.deltaTime;

            if (timer < 0)
            {
                canShoot = true;
            }
        }
        else if (lookHeld)
        {
            canShoot = false;
            timer = shootTime;

            Launch();

        }
    }
    public void set(bool lookHeld, Vector2 direction, Vector2 position)
    {
        this.lookHeld = lookHeld;
        this.direction = direction;
        this.position = position;
        rigidbody2d.MovePosition(position);
    }
    void Launch()
    {
        for (int i = shootMulti; i > 0; i--)
        {
            float shootAngle = Mathf.Atan2(direction.y, direction.x) * (180f / Mathf.PI);
            GameObject projectileObject = Instantiate(ProjectilePrefab, position, Quaternion.Euler(0f, 0f, shootAngle));
            ProjectileScript projectile = projectileObject.GetComponent<ProjectileScript>();
            projectile.Launch(direction, transform.position);
        }
        
    }

}
