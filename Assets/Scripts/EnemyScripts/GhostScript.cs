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
                Launch(direction, rigidbody2D.position, 1, ProjectilePrefab);
                shotTimer = 2f;
                particlePlayed = false;
            }

        }
    }
}
