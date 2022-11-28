using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EnemyScript : MonoBehaviour
{
    public ParticleSystem DeathParticles;

    public float speed;
    public bool vertical;
    public float changeTime = 3.0f;

    public int maxHealth = 30;
    public int health { get { return currentHealth; } }
    int currentHealth;
  
    Color32 colorVal = new Color32(255,255,255,255);
    
    Rigidbody2D rigidbody2D;
    float timer;
    Vector2 direction;
    bool alive = true;
    
    Animator animator;

    float shotTimer;

    bool alertTimer;
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        timer = changeTime;
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;

    }

    void Update()
    {
  

        if (!alive)
        {
            colorVal.a -= (byte)(750 * Time.deltaTime);
            gameObject.GetComponent<Renderer>().material.color = colorVal;
            if(colorVal.a < 3)
            {
                Destroy(gameObject);
            }
            return;
        }

       
        if (colorVal.b < 255)
        {

            if (colorVal.b + (byte)(750 * Time.deltaTime) > 255)
            {
                colorVal.b = 255;
                colorVal.g = 255;
            }
            else
            {
                colorVal.b += (byte)(750 * Time.deltaTime);
                colorVal.g += (byte)(750 * Time.deltaTime);
            }

            gameObject.GetComponent<Renderer>().material.color = colorVal;
        }

        /*  RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
          if (hit.collider != null)
          {
              NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
              if (character != null)
              {
                  character.DisplayDialog();
              }
          }


        RaycastHit2D hit;
        do
        {
            hit = Physics2D.Raycast(transform.position, new Vector2(6,0), 6f, LayerMask.GetMask("Player"));
            
        }
        while (hit.collider!=null);
        if(hit.collider!= null)
        {
            Debug.Log("notNull");
        }
    */


        if (Physics2D.Raycast(transform.position, new Vector2(6, 0), 5f, LayerMask.GetMask("Player")))
        {
            alertTimer = false;
            direction = new Vector2(1, 0);
        }
        else if (Physics2D.Raycast(transform.position, new Vector2(4, 2), 5f, LayerMask.GetMask("Player")))
        {
            alertTimer = false;
            direction = new Vector2(.93f,.46f);
        }
        else if (Physics2D.Raycast(transform.position, new Vector2(3, 3), 5f, LayerMask.GetMask("Player")))
        {
            alertTimer = false;
            direction = new Vector2(.7f, .7f);
        }
        else if (Physics2D.Raycast(transform.position, new Vector2(2, 4), 5f, LayerMask.GetMask("Player")))
        {
            alertTimer = false;
            direction = new Vector2(.46f, .93f);
        }
        else if (Physics2D.Raycast(transform.position, new Vector2(0, 6), 5f, LayerMask.GetMask("Player")))
        {
            alertTimer = false;
            direction = new Vector2(0, 1);
        }
        else if (Physics2D.Raycast(transform.position, new Vector2(-2,4), 5f, LayerMask.GetMask("Player")))
        {
            alertTimer = false;
            direction = new Vector2(-.93f, .46f);
        }
        else if (Physics2D.Raycast(transform.position, new Vector2(-3, 3), 5f, LayerMask.GetMask("Player")))
        {
            alertTimer = false;
            direction = new Vector2(-.7f, .7f);
        }
        else if (Physics2D.Raycast(transform.position, new Vector2(-4, 2), 5f, LayerMask.GetMask("Player")))
        {
            alertTimer = false;
            direction = new Vector2(-.46f, .93f);
        }
        else if (Physics2D.Raycast(transform.position, new Vector2(-6, 0), 5f, LayerMask.GetMask("Player")))
        {
            alertTimer = false;
            direction = new Vector2(-1, 0);
        }
        else if (Physics2D.Raycast(transform.position, new Vector2(-4, -2), 5f, LayerMask.GetMask("Player")))
        {
            alertTimer = false;
            direction = new Vector2(-.93f, -.46f);
        }
        else if (Physics2D.Raycast(transform.position, new Vector2(-3, -3), 5f, LayerMask.GetMask("Player")))
        {
            alertTimer = false;
            direction = new Vector2(-.7f, -.7f);
        }
        else if (Physics2D.Raycast(transform.position, new Vector2(-2, -4), 5f, LayerMask.GetMask("Player")))
        {
            alertTimer = false;
            direction = new Vector2(-.46f, -.93f);
        }
        else if (Physics2D.Raycast(transform.position, new Vector2(0, -6), 5f, LayerMask.GetMask("Player")))
        {
            alertTimer = false;
            direction = new Vector2(0, -1);
        }
        else if (Physics2D.Raycast(transform.position, new Vector2(2, -4), 5f, LayerMask.GetMask("Player")))
        {
            alertTimer = false;
            direction = new Vector2(.93f, -.46f);
        }
        else if (Physics2D.Raycast(transform.position, new Vector2(3, -3), 5f, LayerMask.GetMask("Player")))
        {
            alertTimer = false;
            direction = new Vector2(.7f, -.7f);
        }
        else if (Physics2D.Raycast(transform.position, new Vector2(4, -2), 5f, LayerMask.GetMask("Player")))
        {
            alertTimer = false;
            direction = new Vector2(.46f, -.93f);
        }
        else if(!alertTimer)
        {
            timer = 4f;
            alertTimer = true;
        }
        if (alertTimer)
        {
            timer -= Time.deltaTime;
           
            if (timer < 0)
            {
                direction *= 0;
           
                alertTimer = false;
            }
        }

        
        float shotTimer;


    }

    void FixedUpdate()
    {
        //remember ! inverse the test, so if broken is true !broken will be false and return won’t be executed.
        if (!alive)
        {
            return;
        }

        Vector2 position = rigidbody2D.position;

        position.x += Time.deltaTime * speed * direction.x;
        position.y += Time.deltaTime * speed * direction.y;
     
        animator.SetFloat("Move X", direction.x);
        animator.SetFloat("Move Y", direction.y);




        rigidbody2D.MovePosition(position);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        PlayerScript player = other.gameObject.GetComponent<PlayerScript>();

        if (player != null)
        {
            player.ChangeHealth(-1);
        }
    }

    
        
    

    //Public because we want to call it from elsewhere like the projectile script
    public void Damage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth == 0)
        {

            gameObject.GetComponent<Renderer>().material.color = colorVal;
            DeathParticles.Play();


            alive = false;
            rigidbody2D.simulated = false;

        }
        else
        {


            colorVal = new Color32(255, 0, 0, 255);
        }
      
    }
}