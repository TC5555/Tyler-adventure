using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyObjectData
{
    public List<float> pos = new List<float>();
    public float health { get; set; }
    public bool isActive { get; set; }
}
