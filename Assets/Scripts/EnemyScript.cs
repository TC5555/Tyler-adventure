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
    int direction = 1;
    bool alive = true;

    Animator animator;


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

            
        Physics.Raycast(transform.position, );



        timer -= Time.deltaTime;
        
        if (timer < 0)
        {
            direction = -direction;
            timer = changeTime;
        }
    }

    void FixedUpdate()
    {
        //remember ! inverse the test, so if broken is true !broken will be false and return won’t be executed.
        if (!alive)
        {
            return;
        }

        Vector2 position = rigidbody2D.position;

       
            position.x += Time.deltaTime * speed * direction;
            animator.SetFloat("Move X", direction);
            animator.SetFloat("Move Y", 0);
        
       
          

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