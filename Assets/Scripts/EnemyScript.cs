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

    Rigidbody2D rigidbody2D;
    float timer;
    int direction = 1;
    bool alive = true;

    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        timer = changeTime;
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    void Update()
    {
        //remember ! inverse the test, so if broken is true !broken will be false and return won’t be executed.
        if (!alive)
        {
            return;
        }

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
            DeathParticles.Play();
       
            alive = false;
            rigidbody2D.simulated = false;
        }
        //optional if you added the fixed animation
       // animator.SetTrigger("Fixed");
    }
}