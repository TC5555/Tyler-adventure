
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Game : MonoBehaviour
{

    [SerializeField]
    private GameObject menu;
    [SerializeField]
    private GameObject Progress;
    [SerializeField]
    private GameObject Enemies;
    [SerializeField]
    private int currentScene;

    public bool canSave = true;

    private bool isPaused = false;

    private void Awake()
    {
        StartCoroutine(AutoSave());
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


    public void ChangeScene(int newScene)
    {
        Debug.Log("1");
        SceneManager.LoadScene(newScene);
        currentScene = newScene;
        Debug.Log("2");
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

            currentScene = save.currentScene;

            SceneManager.LoadScene(currentScene);


            // 3
            for (int i = 0; i < save.progress.Count; i++)
            {
                Progress.transform.GetChild(i).gameObject.SetActive(save.progress[i]);
            }

            for (int i = 0; i < save.enemies.Count; i++)
            {
                EnemyObjectData dataE = save.enemies[i];

               EnemyScript enemy = Enemies.transform.GetChild(i).GetComponent<EnemyScript>();

               enemy.transform.position = new Vector2(dataE.pos[0],dataE.pos[1]);

                enemy.currentHealth = dataE.health;

                enemy.gameObject.SetActive(dataE.isActive);
            }

            
           
            Cinemachine.CinemachineConfiner cinemachineConfiner;
            cinemachineConfiner = FindObjectOfType<Cinemachine.CinemachineConfiner>();
            ConfinerScript ConfinerShape = GameObject.Find("CameraConfiner").GetComponent<ConfinerScript>();

            if (ConfinerShape.originalPoints != ConfinerShape.polygonCollider2D.points)
            {
                cinemachineConfiner.m_Damping = 0;

                List<List<float>> bounds = save.bounds;
                List<Vector2> newPoints = new List<Vector2>();


                for (int i = 0; i < bounds.Count; i++)
                {

                    newPoints.Add(new Vector2(bounds[i][0], bounds[i][1]));
                }

                ConfinerShape.polygonCollider2D.SetPath(0, newPoints);
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

        currentScene = save.currentScene;

        foreach (Transform t in Progress.transform)
        {
            GameObject ob = t.gameObject;

            save.progress.Add(ob.activeSelf);
 
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

       

        Cinemachine.CinemachineConfiner cinemachineConfiner;
        cinemachineConfiner = FindObjectOfType<Cinemachine.CinemachineConfiner>();
        ConfinerScript ConfinerShape = GameObject.Find("CameraConfiner").GetComponent<ConfinerScript>();

        if (ConfinerShape.originalPoints != ConfinerShape.polygonCollider2D.points)
        {
            List<List<float>> bounds = new List<List<float>>();

            for (int i = 0; i < ConfinerShape.polygonCollider2D.points.Length; i++)
            {
                Vector2 currentPoints = ConfinerShape.polygonCollider2D.points[i];
                List<float> points = new List<float>();
                points.Add(currentPoints[0]);
                points.Add(currentPoints[1]);
                bounds.Add(points);
            }
            save.bounds = bounds;
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

    IEnumerator AutoSave()
    {

        while (true)
        {
            yield return new WaitForSeconds(1f);
            if (canSave)
            {
                yield return new WaitForSeconds(20f);
                if (canSave)
                {
                    SaveGame();
                }
            }
        }
      
    }
}
