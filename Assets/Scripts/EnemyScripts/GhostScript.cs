using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GhostScript : EnemyScript
{ 
    
    float shotTimer = 2f;
    bool particlePlayed = false;
    public ParticleSystem ShootParticles;

    public GameObject ProjectilePrefab;
   


    void LateUpdate()
    {
        if (!alive)
        {
            Destroy(ShootParticles);
            return;
        }
        
        if (!direction.Equals(new Vector2(0, 0)))
        {
            shotTimer -= Time.deltaTime;
            if (!particlePlayed && shotTimer < 1f && shotTimer > 0f)
            {

                ShootParticles.Play();
                particlePlayed = true;
            }
            else if (shotTimer <= 0f)
            {
                Launch();
                shotTimer = 2f;
                particlePlayed = false;
            }

        }
    }
    void Launch()
    {
       
            float shootAngle = Mathf.Atan2(direction.y, direction.x) * (180f / Mathf.PI);
            GameObject projectileObject = Instantiate(ProjectilePrefab, rigidbody2D.position, Quaternion.Euler(0f, 0f, shootAngle));
            ProjectileScript projectile = projectileObject.GetComponent<ProjectileScript>();
            projectile.Launch(direction, rigidbody2D.position);
        

    }
}
