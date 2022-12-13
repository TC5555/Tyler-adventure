using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Save
{
    public List<bool> triggers = new List<bool>();
    public List<EnemyObjectData> enemies = new List<EnemyObjectData>();
    public PlayerData player = new PlayerData();
    public List<bool> chests = new List<bool>();
}
 
