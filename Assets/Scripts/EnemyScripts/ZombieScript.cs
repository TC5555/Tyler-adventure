using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieScript : EnemyScript
{
    float chargeTimer = 6f;
    public ParticleSystem ChargeParticles;
    bool particlePlayed = false;
    bool charged = false;
    public float chargeSpeedMulti = 1;
    public float chargeCooldown = 6f;

    void LateUpdate()
    {
        if (!alive)
        {
            Destroy(ChargeParticles);
            return;
        }

        
        if (!direction.Equals(new Vector2(0, 0))) 
        {
            chargeTimer -= Time.deltaTime;
            if (!particlePlayed && chargeTimer <= 2.5f && chargeTimer >= 1f)
            {
                ChargeParticles.Play();
                animator.speed /= 5f *chargeSpeedMulti;
                speed /= -5f* chargeSpeedMulti;
                scanning = false;
                particlePlayed = true;
            }
            else if(!charged && chargeTimer < 1f && chargeTimer > 0)
            {
                for (float i = 1f; i <= 60; i++)
                {
                    Vector2 angle = new Vector2(Mathf.Cos(Mathf.Atan2(direction.y, direction.x) - Mathf.PI/8 + (Mathf.PI/4) * (i/60)), Mathf.Sin(Mathf.Atan2(direction.y, direction.x) - Mathf.PI / 8 + (Mathf.PI / 4) * (i / 60)));
           
                    if (Physics2D.Raycast(transform.position, angle, 15f, LayerMask.GetMask("Player")))
                    {                  
                        direction = angle;

                        break;
                    }
                }
                animator.speed *= 25f*chargeSpeedMulti;
                speed *= -25f*chargeSpeedMulti;
                charged = true;
            }
            else if(chargeTimer <= 0)
            {
                charged = false;
                particlePlayed = false;
                scanning = true;
                animator.speed /= 5f * chargeSpeedMulti;
                speed /= 5f * chargeSpeedMulti;
                chargeTimer = chargeCooldown;
                direction *= -1f;

            }
        }
    }
}
