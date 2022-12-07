using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    Vector2 direction;
    public GameObject ProjectilePrefab;
    bool canShoot;
    float timer;
    public float shootTime;
    bool lookHeld;
    public int shootMulti;
    private void Start()
    {
        gameObject.SetActive(false);
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
    public void set(bool lookHeld, Vector2 direction)
    {
        this.lookHeld = lookHeld;
        this.direction = direction;
    }
    void Launch()
    {
        for (int i = shootMulti; i > 0; i--)
        {
            float shootAngle = Mathf.Atan2(direction.y, direction.x) * (180f / Mathf.PI);
            GameObject projectileObject = Instantiate(ProjectilePrefab, transform.position, Quaternion.Euler(0f, 0f, shootAngle));
            ProjectileScript projectile = projectileObject.GetComponent<ProjectileScript>();
            projectile.Launch(direction, transform.position);
        }
        
    }

}
