using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public List<float> pos = new List<float>();
    public string[] weapons { get; set; }

    public int maxHealth { get; set; }
    public float health { get; set; }
    public int maxStamina { get; set; }

    public float stamina { get; set; }
    
    public int healAmount { get; set; }
    public int healthHealed { get; set; }
}

