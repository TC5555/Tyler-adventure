using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIWeaponDisplayScript : MonoBehaviour
{
    public static UIWeaponDisplayScript instance { get; private set; }

    private void Awake()
    {
        instance = this;
    }

    public void SetValue(Sprite WeaponIcon)
    {
        
        gameObject.GetComponent<Image>().sprite = WeaponIcon;

    }
}
