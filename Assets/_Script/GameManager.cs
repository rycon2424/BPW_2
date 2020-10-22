using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static int sceneNumber = 0;

    void Start()
    {
        
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SceneManager.LoadScene(sceneNumber);
        }
    }

    public static void RestartLevel()
    {
        SceneManager.LoadScene(GameManager.sceneNumber);
    }

}
