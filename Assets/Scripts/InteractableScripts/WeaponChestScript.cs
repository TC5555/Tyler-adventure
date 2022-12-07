using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponChestScript : InteractableScript
{
    public GameObject WeaponAdded;
    public override void Interact() => addWeapon();

    bool isOpen = false;

 public void addWeapon()
{
        if (!isOpen)
        {
           // GameObject Player = GameObject.FindGameObjectWithTag("Player");
            GameObject W = Instantiate(WeaponAdded);
            //PlayerScript PlayerScr = Player.GetComponent<PlayerScript>();
            W.transform.SetParent(GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().transform,false);
            isOpen = true;
        }
}

}