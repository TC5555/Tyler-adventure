using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public float speed = 3.0f;

    public int maxHealth = 5;
    //public ParticleSystem DamageParticles;
GameObject ProjectilePrefab;


    AudioSource audioSource;
    public int health { get { return currentHealth; } }
    int currentHealth;

    public float timeInvincible = 2.0f;
    bool isInvincible;
    float invincibleTimer;

    float timeShoot = 0.5f;
    bool canShoot;
    float shootTimer;
    int shootMulti = 1;

    Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;
    float horizontalShoot;
    float verticalShoot;
    Animator animator;
    Vector2 moveDirection = new Vector2(1, 0);
    Vector2 lookDirection = new Vector2(1, 0);
    bool lookHeld;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        currentHealth = maxHealth;
        ProjectilePrefab = GameObject.Find("Projectile1");
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        horizontalShoot = Input.GetAxis("HorizontalShoot");
        verticalShoot = Input.GetAxis("VerticalShoot");

       
        Vector2 move = new Vector2(horizontal, vertical);


        Vector2 shoot = new Vector2(horizontalShoot, verticalShoot);
        
        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {

            moveDirection.Set(move.x, move.y);
            moveDirection.Normalize();
        }

        if (!Mathf.Approximately(shoot.x, 0.0f) || !Mathf.Approximately(shoot.y, 0.0f))
        {
            lookHeld = true;
            lookDirection.Set(shoot.x, shoot.y);
            lookDirection.Normalize();
        }
        else
        {

            lookHeld = false;
        }
        

        //  animator.SetFloat("Look X", moveDirection.x);
        // animator.SetFloat("Look Y", moveDirection.y);
        // animator.SetFloat("Speed", move.magnitude);
        
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }


        if(canShoot)
            {
            if (lookHeld)
            {
                canShoot = false;
                shootTimer = timeShoot;

                Launch();
            }
            else if (Input.GetKeyDown("1"))
            {
                timeShoot = 0.5f;
                shootMulti = 1;
                ProjectilePrefab = GameObject.Find("Projectile1");
            }
            else if (Input.GetKeyDown("2"))
            {
                timeShoot = .1f;
                shootMulti = 3;
                ProjectilePrefab = GameObject.Find("Projectile2");
            }
        }

       
        
    }

    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;

        rigidbody2d.MovePosition(position);

        if (!canShoot)
        {

            shootTimer -= Time.deltaTime;
   
            if (shootTimer < 0)
            {
                canShoot = true;
            }
        }
      
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvincible)
                return;
             
            isInvincible = true;
            invincibleTimer = timeInvincible;
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        Debug.Log(currentHealth);
        //Instantiate(DamageParticles, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
        //DamageParticles.Play();
       // UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
    }

    void Launch()
    {

        for (int i = shootMulti; i > 0; i--)
        {
            GameObject projectileObject = Instantiate(ProjectilePrefab, rigidbody2d.position, Quaternion.identity);

         ProjectileScript projectile = projectileObject.GetComponent<ProjectileScript>();
        
            projectile.Launch(lookDirection, transform.position);
        }

        //animator.SetTrigger("Launch");
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}