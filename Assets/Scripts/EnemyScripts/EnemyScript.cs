using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EnemyScript : launchScript
{
    public ParticleSystem DeathParticles;


    public float speed;


    public int maxHealth;
    public int health { get { return currentHealth; } }
    protected int currentHealth;
  
    protected Color32 colorVal = new Color32(255,255,255,255);
    
    protected new Rigidbody2D rigidbody2D;
    protected float timer;
    protected Vector2 direction;
    public bool alive = true;
    public GameObject pickupPrefab;
    public float scanRange;

    protected float divisions = 32;

    public Animator animator;
    protected bool scanning = true;

    public float percentHealthDropChance;
    public int amountHealthDropped;

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
            if (colorVal.a - (byte)(750 * Time.deltaTime) < 0)
            {
                Destroy(gameObject);
            }

            colorVal.a -= (byte)(750 * Time.deltaTime);
            gameObject.GetComponent<Renderer>().material.color = colorVal;
               
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

        if (scanning)
        {
            for (float i = 0f; i <= divisions; i++)
            {
                Vector2 angle = new Vector2(Mathf.Cos(2f * Mathf.PI * i / divisions), Mathf.Sin(2f * Mathf.PI * i / divisions));
                // Debug.Log("I: " + 6.283f * i / divisions + " Quaternion: " + Mathf.Cos(6.283f * i / divisions) + " " + Mathf.Sin(6.283f * i / divisions));
                if (Physics2D.Raycast(transform.position, angle, scanRange, LayerMask.GetMask("Player")))
                {
                    alertTimer = false;
                    direction = angle;

                    break;
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
        }
    }

    void FixedUpdate()
    {
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
            player.ChangeHealth(-2);
        }
    }

    void DropHealth()
    {
        for (int i = amountHealthDropped; i > 0; i--)
        {
            float Rand = Random.Range(0, 100);
            if (Rand <= percentHealthDropChance)
            {
                GameObject HealthPickupObject = Instantiate(pickupPrefab, rigidbody2D.position, Quaternion.identity);
            }
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
            DropHealth();

            alive = false;
            rigidbody2D.simulated = false;

        }
        else
        {


            colorVal = new Color32(255, 0, 0, 255);
        }
      
    }
}
