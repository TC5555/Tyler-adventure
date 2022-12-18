using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChangerScript : MonoBehaviour
{
    public int newScene;
    private void Awake()
    {
        Game game = GameObject.Find("Main Camera").GetComponent<Game>();
        game.ChangeScene(newScene);
    }
}
