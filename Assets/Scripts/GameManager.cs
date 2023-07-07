using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    //Gameobjects
    [SerializeField] private GameObject GameOverUI;
    [SerializeField] private GameObject WinLevelUI;
    [SerializeField] private GameObject PowerUpUI;
    [SerializeField] private GameObject PauseUI;

    //var
    public string NextLevel;
    public int CountEnemies;

    public bool isEnd;
<<<<<<< Updated upstream
    public bool isOnTimeout;
=======
    public bool Disabled;
>>>>>>> Stashed changes
    public bool isPaused;
    public bool isGameOver;

    public SceneScript sceneScript;

    private void Awake()
    {
        instance = this;
        Time.timeScale = 1;
<<<<<<< Updated upstream
        isOnTimeout = false;
=======
        Disabled = false;
>>>>>>> Stashed changes
    }

    private void Start()
    {
        isEnd = false;
    }

    //Do function here
    private void Update()
    {
        GameObject[] ActiveEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        CountEnemies = ActiveEnemies.Length;

        if (CountEnemies == 0)
        {
            isEnd = true;
        }

        if (PlayerVar.PowerUp)
        {
            PowerUpUI.SetActive(true);
        }
        else
        {
            PowerUpUI.SetActive(false);
        }     
    }

    //Pause the Game
    public void PauseGame()
    {
        isPaused = !isPaused;
        isOnTimeout = !isOnTimeout;

        if (!isPaused)
        {
            Time.timeScale = 1;
            PauseUI.SetActive(false);
            Disabled = false;
        }
        else
        {
            Time.timeScale = 0;
            PauseUI.SetActive(true);
            Disabled = true;
        }
    }

    //Win Condition
    public void Win()
    {
        isOnTimeout = true;
        WinLevelUI.SetActive(true);
        Time.timeScale = 0;
        Disabled = true;
    }
    //Lose Condition
    public void Die()
    {
        isOnTimeout = false;
        GameOverUI.SetActive(true);
        Time.timeScale = 0;
    }
    //Save the game
    public void NextLv(string key)
    {
        PlayerPrefs.SetInt(key, 1);
    }
    //Quit the Game
    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quitting");
    }
}
