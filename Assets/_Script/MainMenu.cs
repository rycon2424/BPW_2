using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject firstMenu;
    public GameObject secondMenu;
    [Space]
    public InputField inf;
    public Text highscore;
    public Button startGame;
    public bool mainMenu;

    private void Start()
    {
        highscore.text = "Enter a seed";
        mainMenu = true;
    }

    void Update()
    {
        if (mainMenu)
        {
            if (Input.anyKey)
            {
                mainMenu = false;
                firstMenu.SetActive(false);
                secondMenu.SetActive(true);
            }
        }
    }

    public void CheckForHighScore()
    {
        if (inf.text == "")
        {
            highscore.text = "Enter a seed";
            startGame.interactable = false;
            return;
        }
        else
        {
            startGame.interactable = true;
        }
        SeedSingleTon.instance.seed = int.Parse(inf.text);
        if (PlayerPrefs.HasKey(inf.text))
        {
            highscore.text = "Highscore = " + PlayerPrefs.GetString(inf.text) + " seconds!";
        }
        else
        {
            highscore.text = "No highscore on this seed";
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }
}
