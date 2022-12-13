
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{

    [SerializeField]
    private GameObject menu;
    [SerializeField]
    private GameObject Triggers;
    [SerializeField]
    private GameObject Enemies;
    [SerializeField]
    private GameObject Chests;

    private bool isPaused = false;

    private void Awake()
    {
        Unpause();
    }

 
    public void Pause()
    {
        menu.SetActive(true);
        Cursor.visible = true;
        Time.timeScale = 0;
        isPaused = true;
    }

    public void Unpause()
    {
        menu.SetActive(false);
        Cursor.visible = false;
        Time.timeScale = 1;
        isPaused = false;
    }

    public bool IsGamePaused()
    {
        return isPaused;
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (isPaused)
            {
                Unpause();
            }
            else
            {
                Pause();
            }
        }
    }

    public void SaveGame()
    {
        // 1
        Save save = CreateSaveGameObject();
        

        // 2
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gamesave.save");
        bf.Serialize(file, save);
        Unpause();
        Debug.Log("Game Saved");
    }

    public void LoadGame()
    {
        // 1
        if (File.Exists(Application.persistentDataPath + "/gamesave.save"))
        {

            // 2
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
            Save save = (Save)bf.Deserialize(file);
            file.Close();

            // 3
            for (int i = 0; i < save.triggers.Count; i++)
            {
                Triggers.transform.GetChild(i).gameObject.SetActive(save.triggers[i]);
            }

            for (int i = 0; i < save.chests.Count; i++)
            {
                Chests.transform.GetChild(i).gameObject.SetActive(save.chests[i]);
            }


            for (int i = 0; i < save.enemies.Count; i++)
            {
                EnemyObjectData dataE = save.enemies[i];

               EnemyScript enemy = Enemies.transform.GetChild(i).GetComponent<EnemyScript>();

               enemy.transform.position = new Vector2(dataE.pos[0],dataE.pos[1]);

                enemy.currentHealth = dataE.health;

                enemy.gameObject.SetActive(dataE.isActive);
            }


            PlayerData dataP = save.player;

            PlayerScript playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();

            playerScript.transform.position = new Vector2(dataP.pos[0], dataP.pos[1]);

            playerScript.maxHealth = dataP.maxHealth;

            playerScript.currentHealth = dataP.health;

            playerScript.maxStamina = dataP.maxStamina;

            playerScript.currentStamina = dataP.stamina;

            playerScript.healthHealed = dataP.healthHealed;

            playerScript.healAmount = dataP.healAmount;


            playerScript.updateUI();

            Debug.Log("Game Loaded");

            Unpause();
        }
        else
        {
            Debug.Log("No game saved!");
        }
    }

    private Save CreateSaveGameObject()
    {
        Save save = new Save();

        foreach (Transform t in Triggers.transform)
        {
            GameObject ob = t.gameObject;

            save.triggers.Add(ob.activeSelf);
 
        }

        foreach (Transform t in Chests.transform)
        {

            WeaponChestScript ob = t.gameObject.GetComponent<WeaponChestScript>();

            save.chests.Add(ob.isOpen);
        }


        foreach (Transform t in Enemies.transform)
        {
            EnemyObjectData dataE = new EnemyObjectData();

            EnemyScript enemy = t.gameObject.GetComponent<EnemyScript>();

            dataE.pos.Add(enemy.transform.position.x);
            dataE.pos.Add(enemy.transform.position.y);

            dataE.health = enemy.currentHealth;

            dataE.isActive = enemy.isActiveAndEnabled;

            save.enemies.Add(dataE);
        }

        PlayerData dataP = new PlayerData();

        PlayerScript playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();

        dataP.pos.Add(playerScript.transform.position.x);
        dataP.pos.Add(playerScript.transform.position.y);

        dataP.maxHealth = playerScript.maxHealth;

        dataP.health = playerScript.currentHealth;

        dataP.maxStamina = playerScript.maxStamina;

        dataP.stamina = playerScript.currentStamina;

        dataP.healthHealed = playerScript.healthHealed;

        dataP.healAmount = playerScript.healAmount;

        save.player = dataP;


        return save;
    }
}
