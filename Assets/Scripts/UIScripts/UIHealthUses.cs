using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHealthUses : UIBarScript
{
    public static UIHealthUses instance { get; private set; }

    private void Awake()
    {
        instance = this;
    }
}