using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    float speed = 3.0f;

    public int maxHealth = 10;

    int maxStamina = 100;
    float currentStamina;
    bool sprinting = false;
    float isSprinting;
    bool dodging = false;
    float isDodging;

    int healAmount;
    int healMax = 4;
    float isHealing;
    float healTimer;
    float healTime = 1f;
    bool healing = false;

    float isSwitching;
    float switchingTimer;
    float switchingCooldown = 3f;

    public ParticleSystem DodgeParticles;
    public ParticleSystem DamageParticles;
    public ParticleSystem HealParticles;

    GameObject ActiveWeapon;
    WeaponScript WeaponScr;

    AudioSource audioSource;
    public int health { get { return currentHealth; } }
    int currentHealth;

    public float timeInvincible = 2.0f;
    bool isInvincible;
    float invincibleTimer;
    float staminaTimer;
    float staminaCooldown = 1f;
    float dodgeTimer;
    float dodgeTime = .2f;
    float dodgeCooldown = 1f;

    Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;
    float horizontalShoot;
    float verticalShoot;
    Animator animator;
    Vector2 moveDirection = new Vector2(1, 0);
    Vector2 lookDirection = new Vector2(1, 0);
    bool lookHeld;
    bool moveHeld;

    ArrayList Weapons = new ArrayList();
    int currentWeapon = 0;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        currentHealth = maxHealth;
        currentStamina = maxStamina;
        ActiveWeapon = GameObject.Find("BasicWeapon");
        WeaponScr = ActiveWeapon.GetComponent<WeaponScript>();

        UIHealingTextScript.instance.SetValue(healMax);
        healAmount = healMax;

        UIWeaponDisplayScript.instance.SetValue(ActiveWeapon.GetComponent<SpriteRenderer>().sprite);
        Weapons.Add("BasicWeapon");
        Weapons.Add("ShotgunWeapon");
      
    }
    


    // Update is called once per frame
    void Update()
    {
        
        if (!dodging)
        {
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");

            horizontalShoot = Input.GetAxis("HorizontalShoot");
            verticalShoot = Input.GetAxis("VerticalShoot");

            Vector2 move = new Vector2(horizontal, vertical);


            Vector2 shoot = new Vector2(horizontalShoot, verticalShoot);

            if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
            {
                moveHeld = true;
                moveDirection.Set(move.x, move.y);
                moveDirection.Normalize();
            }
            else
            {
                moveHeld = false;
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

        if (sprinting)
        {
            currentStamina -= 20f * Time.deltaTime;
            UIStaminaBarScript.instance.SetValue(currentStamina / (float)maxStamina);
            if (currentStamina <= 0)
            {
                staminaTimer = staminaCooldown;
            }
        }
        else if (staminaTimer <= 0 )
        {

            if (currentStamina + 15f * Time.deltaTime > maxStamina)
            {
                currentStamina = maxStamina;
            }
            else
            {
                currentStamina += 15f * Time.deltaTime;
            }
            UIStaminaBarScript.instance.SetValue(currentStamina / (float)maxStamina);
            
        }

        staminaTimer -= Time.deltaTime;

        if (dodgeTimer<=0 && dodging)
        {
            dodging = false;
            speed /= 5f;
            dodgeTimer = dodgeCooldown;
        }

        dodgeTimer -= Time.deltaTime;

        if(healTimer <= 0 && healing)
        {
            healAmount -= 1;
            UIHealingTextScript.instance.SetValue(healAmount);
            ChangeHealth(1);
            healing = false;
            speed *= 2f;
        }

        healTimer -= Time.deltaTime;

        isHealing = Input.GetAxis("Heal");

        if (healAmount >= 0 && isHealing > .2f && !healing && !sprinting && health != maxHealth)
        {
            Debug.Log("healing");
            healing = true;
            healTimer = healTime;
            speed /= 2f;
        }

        isSwitching = Input.GetAxis("WeaponSelect");

        if(!Mathf.Approximately(isSwitching, 0.0f) && switchingTimer <= 0)
        {
           // Debug.Log("Switching " + Weapons.Count + " " + currentWeapon + " " + isSwitching);
            if(isSwitching > 0)
            {
                if (Weapons.Count == currentWeapon -1)
                {
                    currentWeapon = 0;

                }
                else
                {
                    currentWeapon++;
                }
            }
            else
            {
                if (currentWeapon -1 < 0)
                {
                    currentWeapon = Weapons.Count - 1;
                }
                else
                {
                    currentWeapon--;
                }
            }
            WeaponScr.set(false, new Vector2(0, 0), new Vector2(-100, 0)); 
        
            ActiveWeapon = GameObject.Find((string)Weapons.ToArray()[currentWeapon]);
            WeaponScr = ActiveWeapon.GetComponent<WeaponScript>();
            UIWeaponDisplayScript.instance.SetValue(ActiveWeapon.GetComponent<SpriteRenderer>().sprite);
            switchingTimer = switchingCooldown;
        }

        switchingTimer -= Time.deltaTime;

    }

    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        
        if (moveHeld&&!healing)
        {
   
            isSprinting = Input.GetAxis("Sprint");

            isDodging = Input.GetAxis("Dodge");

            if (isDodging > .2f && !dodging && currentStamina > 30 && dodgeTimer <= 0)
            {
                dodging = true;
                dodgeTimer = dodgeTime;
                lookHeld = false;
                speed *= 5;
                currentStamina -= 40f;
                UIStaminaBarScript.instance.SetValue(currentStamina / (float)maxStamina);
                if (currentStamina <= 0)
                {
                    staminaTimer = staminaCooldown *2f;
                }
                DodgeParticles.Play();
            }

            if (isSprinting > .2f && !sprinting && staminaTimer <= 0)
            {
                
                speed *= 1.5f;
                sprinting = true;
            }
            if ((isSprinting < .2f || staminaTimer >= 0) && sprinting)
            {
                speed /= 1.5f;
                sprinting = false;
            }
        }

      
        position.x += speed * horizontal * Time.deltaTime;
        position.y += speed * vertical * Time.deltaTime;

        rigidbody2d.MovePosition(position);

        WeaponScr.set(lookHeld, lookDirection, position);
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvincible)
                return;
             
            isInvincible = true;
            invincibleTimer = timeInvincible;
            DamageParticles.Play();
        }
        else
        {
            HealParticles.Play();
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        Debug.Log(currentHealth); 
       
        UIHealthBarScript.instance.SetValue(currentHealth / (float)maxHealth);
    }

    

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}