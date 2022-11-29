using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EnemyScript : MonoBehaviour
{
    public ParticleSystem DeathParticles;
    public ParticleSystem ShootParticles;

    public float speed;


    public GameObject ProjectilePrefab;
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
    bool canShoot = true;
    bool alertTimer;
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();

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

        for(int i = 0; i < 6; i++)
        {
            if (Physics2D.Raycast(transform.position, new Vector2(6-i, 0+i), 5f, LayerMask.GetMask("Player")))
            {
                alertTimer = false;
                direction = new Vector2(6-i, 0+i) / 6;
            }
            else if (Physics2D.Raycast(transform.position, new Vector2(0-i, 6-i), 5f, LayerMask.GetMask("Player")))
            {
                alertTimer = false;
                direction = new Vector2(0 - i, 6 - i) / 6;
            }
            else  if (Physics2D.Raycast(transform.position, new Vector2(-6+i, 0-i), 5f, LayerMask.GetMask("Player")))
            {
                alertTimer = false;
                direction = new Vector2(-6 + i, 0 - i) / 6;
            }
            else if (Physics2D.Raycast(transform.position, new Vector2(0+i, -6+i), 5f, LayerMask.GetMask("Player")))
            {
                alertTimer = false;
                direction = new Vector2(0 + i, -6 + i) / 6;
            }
            else if (!alertTimer)
            {
                timer = 4f;
                alertTimer = true;
            }

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

        if (!direction.Equals(new Vector2(0, 0)) && canShoot)
        {
            Debug.Log("shoot " + direction);
            shotTimer = 2f;
            canShoot = false;
            ShootParticles.Play();
            Launch();
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




    void Launch()
    {

        
            GameObject projectileObject = Instantiate(ProjectilePrefab, transform.position, Quaternion.identity);

            ProjectileScript projectile = projectileObject.GetComponent<ProjectileScript>();

            projectile.Launch(direction, transform.position);
       

        //animator.SetTrigger("Launch");
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