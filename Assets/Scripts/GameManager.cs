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
    public bool isPaused;
    public bool isGameOver;

    public SceneScript sceneScript;

    private void Awake()
    {
        instance = this;
        Time.timeScale = 1;
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

        if (PlayerVar.PowerUp)
        {
            PowerUpUI.SetActive(true);
        }
        else
        {
            PowerUpUI.SetActive(false);
        }

        if(CountEnemies == 0)
        {
            isEnd = true;
        }     
    }

    //Pause the Game
    public void PauseGame()
    {
        isPaused = !isPaused;

        if (!isPaused)
        {
            Time.timeScale = 1;
            PauseUI.SetActive(false);
        }
        else
        {
            Time.timeScale = 0;
            PauseUI.SetActive(true);
        }
    }

    //Win Condition
    public void Win()
    {
        WinLevelUI.SetActive(true);
        Time.timeScale = 0;
    }
    //Lose Condition
    public void Die()
    {
        GameOverUI.SetActive(true);
        Time.timeScale = 0;
    }
    //Save the game
    public void NextLv(string key)
    {
        sceneScript.LoadScene(NextLevel);
        PlayerPrefs.SetInt(key, 1);
    }
    //Quit the Game
    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quitting");
    }
}
