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

        if (isPaused)
        {
            PauseGame();
        }
        else
        {
            PressContinue();
        }
    }
    //Pause Game
    public void PauseGame()
    {
        PauseUI.SetActive(true);
        Time.timeScale = 0;
    }
    public void PressContinue()
    {
        isPaused = false;
        PauseUI.SetActive(false);
        Time.timeScale = 1;
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
    //Restarting the Game
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
