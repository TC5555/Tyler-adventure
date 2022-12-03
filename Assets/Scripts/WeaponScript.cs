using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    protected Vector2 direction;
    public GameObject ProjectilePrefab;
    protected Vector2 position;
    protected bool canShoot;
    protected float timer;
    protected float shootTime;
    bool lookHeld;

    void update()
    {
        if(!canShoot)
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
    }
    void Launch()
    {
        float shootAngle = Mathf.Atan2(direction.y, direction.x) * (180f / Mathf.PI);
        GameObject projectileObject = Instantiate(ProjectilePrefab, position, Quaternion.Euler(0f, 0f, shootAngle));
        ProjectileScript projectile = projectileObject.GetComponent<ProjectileScript>();
        projectile.Launch(direction, transform.position);
    }

}
