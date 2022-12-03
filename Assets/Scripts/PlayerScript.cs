using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public float speed = 3.0f;

    public int maxHealth = 5;

    GameObject ActiveWeapon;
    WeaponScript Weapon;

    AudioSource audioSource;
    public int health { get { return currentHealth; } }
    int currentHealth;

    public float timeInvincible = 2.0f;
    bool isInvincible;
    float invincibleTimer;


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
        ActiveWeapon = GameObject.Find("BasicWeapon");
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
        
        if (Input.GetKey("1"))
        {
            Weapon = ActiveWeapon.GetComponent<WeaponScript>();
            Weapon.set(false, new Vector2(0,0), new Vector2(-100, 0));
            ActiveWeapon = GameObject.Find("BasicWeapon");
        }
        else if (Input.GetKey("2"))
        {
            Weapon = ActiveWeapon.GetComponent<WeaponScript>();
            Weapon.set(false, new Vector2(0, 0), new Vector2(-100, 0));
            ActiveWeapon = GameObject.Find("ShotgunWeapon");
        }
        else if (Input.GetKey("3"))
        {
            Weapon = ActiveWeapon.GetComponent<WeaponScript>();
            Weapon.set(false, new Vector2(0, 0), new Vector2(-100, 0));
            ActiveWeapon = GameObject.Find("SniperWeapon");
        }
    }

    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        position.x += speed * horizontal * Time.deltaTime;
        position.y += speed * vertical * Time.deltaTime;

        rigidbody2d.MovePosition(position);

        Weapon = ActiveWeapon.GetComponent<WeaponScript>();
        Weapon.set(lookHeld, lookDirection, position);
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

    

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}