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

    float isInteracting;

    int healthHealed = 2;
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

    public bool healUsesText;

    public int weaponChildrenStart;

    int currentWeapon;
    
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        currentHealth = maxHealth;
        currentStamina = maxStamina;
        ActiveWeapon = transform.GetChild(weaponChildrenStart).gameObject;
        ActiveWeapon.SetActive(true);
        currentWeapon = weaponChildrenStart;

        WeaponScr = ActiveWeapon.GetComponent<WeaponScript>();

        if (healUsesText)
        {
            UIHealingTextScript.instance.gameObject.SetActive(true);
            UIHealingTextScript.instance.SetValue(healMax);
        }
        healAmount = healMax;

        UIWeaponDisplayScript.instance.SetValue(ActiveWeapon.GetComponent<SpriteRenderer>().sprite);

    }
    
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

            if ((!Mathf.Approximately(shoot.x, 0.0f) || !Mathf.Approximately(shoot.y, 0.0f)) && !healing)
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
            UIStaminaBarScript.instance.SetValue(true, currentStamina / (float)maxStamina);
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
            UIStaminaBarScript.instance.SetValue(true,currentStamina / (float)maxStamina);
            
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
            if (UIHealingTextScript.instance.isActiveAndEnabled)
            {
                UIHealingTextScript.instance.SetValue(healAmount);
            }
            UIHealthUses.instance.SetValue(false,(float)healAmount/healMax);
            ChangeHealth(healthHealed);
            healing = false;
            speed *= 2f;
        }

        healTimer -= Time.deltaTime;

        isHealing = Input.GetAxis("Heal");

        if (healAmount > 0 && isHealing > .2f && !healing && !sprinting && health != maxHealth)
        {
            Debug.Log("healing");
            healing = true;
            healTimer = healTime;
            speed /= 2f;
        }

        isSwitching = Input.GetAxis("WeaponSelect");

        if(!Mathf.Approximately(isSwitching, 0.0f) && switchingTimer <= 0)
        {
            if(isSwitching > 0)
            {
                if (currentWeapon == transform.childCount -1)
                {
                    currentWeapon = weaponChildrenStart;

                }
                else
                {
                    currentWeapon++;
                }
            }
            else
            {
                if (currentWeapon == weaponChildrenStart)
                {
                    currentWeapon = transform.childCount - 1;
                }
                else
                {
                    currentWeapon--;
                }
            }
            ActiveWeapon.SetActive(false);
            Debug.Log(currentWeapon);
            ActiveWeapon = transform.GetChild(currentWeapon).gameObject;
            WeaponScr = ActiveWeapon.GetComponent<WeaponScript>();
            UIWeaponDisplayScript.instance.SetValue(ActiveWeapon.GetComponent<SpriteRenderer>().sprite);
            ActiveWeapon.SetActive(true);
            switchingTimer = switchingCooldown;
        }

        switchingTimer -= Time.deltaTime;

        isInteracting = Input.GetAxis("Interact");

        if(isInteracting > .1f)
        {
            for (float i = 0f; i <= 8; i++)
            {
                Vector2 angle = new Vector2(Mathf.Cos(2f * Mathf.PI * i / 8), Mathf.Sin(2f * Mathf.PI * i / 8));
                // Debug.Log("I: " + 6.283f * i / divisions + " Quaternion: " + Mathf.Cos(6.283f * i / divisions) + " " + Mathf.Sin(6.283f * i / divisions));
                if (Physics2D.Raycast(transform.position, angle, 3f, LayerMask.GetMask("Interactable")))
                {
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, angle, 3f, LayerMask.GetMask("Interactable"));

                    InteractableScript interactable = hit.collider.GetComponent<InteractableScript>();

                    interactable.Interact();

                    break;
                }
              
            }
        }

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
                UIStaminaBarScript.instance.SetValue(true,currentStamina / (float)maxStamina);
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
        }
        if ((isSprinting < .2f || staminaTimer >= 0) && sprinting)
        {
            speed /= 1.5f;
            sprinting = false;
        }


        position.x += speed * horizontal * Time.deltaTime;
        position.y += speed * vertical * Time.deltaTime;

        rigidbody2d.MovePosition(position);

        WeaponScr.set(lookHeld, lookDirection);
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
       
        UIHealthBarScript.instance.SetValue(true,currentHealth / (float)maxHealth);
    }

    public void changeValues(Vector2 pos, int maxHealth, int maxStamina, int healthHealed)
    {
        rigidbody2d.position = pos;
        this.maxHealth = maxHealth;
        this.maxStamina = maxStamina;
        this.healthHealed = healthHealed;
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}