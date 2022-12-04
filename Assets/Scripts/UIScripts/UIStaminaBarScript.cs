using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UIStaminaBarScript : UIBarScript
{
    public static UIStaminaBarScript instance { get; private set; }

    private void Awake()
    {
        instance = this;
    }
}