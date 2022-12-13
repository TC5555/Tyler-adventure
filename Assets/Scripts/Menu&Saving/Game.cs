
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
    private GameObject progress;
    private bool isPaused = false;
    public PlayerData playerData { get;set; }

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

        PlayerData data = save.player;

        GameObject player = GameObject.FindGameObjectWithTag("player");
        
    

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
            for (int i = 0; i < save.alive.Count; i++)
            {
                
                progress.transform.GetChild(i).gameObject.SetActive(save.alive[i]) ;
            }
            /*foreach (string triggerActive in save.alive)
            {
                
                progress.transform.Find("0");
            }*/

            PlayerData data = save.player;

            GameObject player = GameObject.FindGameObjectWithTag("player");

            player.GetComponent<PlayerScript>().changeValues(new Vector2(data.pos[1], data.pos[2]), data.maxHealth, data.maxStamina, data.healthHealed);
          

            Debug.Log("Game Loaded");

            Unpause();
        }
        else
        {
            Debug.Log("No game saved!");
        }
    }

    public void SaveAsJSON()
    {
        Save save = CreateSaveGameObject();
        string json = JsonUtility.ToJson(save);

        Debug.Log("Saving as JSON: " + json);

        save = JsonUtility.FromJson<Save>(json);

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gamesave.save");
        bf.Serialize(file, save);
        Debug.Log("Saving as Save: " + save);

    }

    private Save CreateSaveGameObject()
    {
        Save save = new Save();
        foreach (Transform t in progress.transform)
        {
            GameObject ob = t.gameObject;
            if (
                ob.activeSelf)
            {
                save.alive.Add(ob.GetComponent<Rigidbody2D>().IsAwake());
          
            }
        }

        return save;
    }
}
