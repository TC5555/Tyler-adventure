using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public float[] pos { get; set; }
    public string[] weapons { get; set; }

    public int maxHealth { get; set; }
    
    public int maxStamina { get; set; }

    public int healthHealed { get; set; }
}

