using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UIHealingTextScript : MonoBehaviour
{
    public static UIHealingTextScript instance { get; private set; }

    private void Awake()
    {
        instance = this;
        gameObject.SetActive(false);
    }

    public void SetValue(float value)
    { 
        gameObject.GetComponent<TextMeshProUGUI>().SetText(value.ToString());
        
    }
   
}
