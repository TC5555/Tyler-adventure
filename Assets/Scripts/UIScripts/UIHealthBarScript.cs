using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHealthBarScript : UIBarScript
{
    public static UIHealthBarScript instance { get; private set; }
    private void Awake()
    {
        instance = this;
    }
}