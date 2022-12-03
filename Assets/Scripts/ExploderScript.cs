using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExploderScript : EnemyScript
{
    float explosionTimer = 8f;
    public ParticleSystem explosionEffects;

    void LateUpdate()
    {
        if (!alive)
        {
            return;
        }

        if (!direction.Equals(new Vector2(0, 0)))
        {
            explosionTimer -= Time.deltaTime;

            if (colorVal.Equals(new Color32(255,255,255,255)))
            {
                colorVal.b -= (byte)(255  * explosionTimer / 8);
                colorVal.g -= (byte)(255  * explosionTimer / 8);
            }

            speed += .5f * Time.deltaTime;
            if (explosionTimer <= 2f)
            {
                scanning = false;
            }
            if (explosionTimer <= 0)
            {
                for (float i = 0f; i <= 64; i++)
                {
                    Vector2 angle = new Vector2(Mathf.Cos(2f * Mathf.PI * i / divisions), Mathf.Sin(2f * Mathf.PI * i / divisions));

                    if (Physics2D.Raycast(transform.position, angle, 4f, LayerMask.GetMask("Player")))
                    {
                        RaycastHit2D hit = Physics2D.Raycast(transform.position, angle, scanRange, LayerMask.GetMask("Player"));

                        PlayerScript player = hit.collider.GetComponent<PlayerScript>();

                        player.ChangeHealth(-1);                        

                        break;
                    }      
                }
                explosionEffects.Play();
                alive = false;
            }
        }
    }
}
