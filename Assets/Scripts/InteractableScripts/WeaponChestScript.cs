using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponChestScript : InteractableScript
{
    public string WeaponAdded;
    public override void Interact() => addWeapon();

    bool isOpen = false;

 public void addWeapon()
{
        if (!isOpen)
        {
            GameObject Player = GameObject.FindGameObjectWithTag("Player");
            PlayerScript PlayerScr = Player.GetComponent<PlayerScript>();
            PlayerScr.Weapons.Add(WeaponAdded);
            isOpen = true;
        }
}

}