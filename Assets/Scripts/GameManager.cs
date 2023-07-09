using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    //Gameobjects
    [SerializeField] private GameObject GameOverUI;
    [SerializeField] private GameObject WinLevelUI;
    [SerializeField] private GameObject PowerUpUI;
    [SerializeField] private GameObject PauseUI;
    [SerializeField] private TextMeshProUGUI ScoreText;

    public TextMeshProUGUI highscoretext;
    public int score = 0;
    public int highscore = 0;

    //var
    public string currentlevel;
    public int CountEnemies;

    public bool isStart;
    public bool isEnd;
    public bool isPaused;
    public bool isGameOver;
    public bool isDisbaled;

    public SceneScript sceneScript;

    private void Awake()
    {
        //When awake
        instance = this;
        Time.timeScale = 1;
        isDisbaled = false;
    }

    private void Start()
    {
        isEnd = false;
        //Load the highscore
        highscore = PlayerPrefs.GetInt(currentlevel);
        ScoreText.text = score.ToString() + " G";
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
            isDisbaled = false;
            Time.timeScale = 1;
            PauseUI.SetActive(false);
        }
        else
        {
            isDisbaled = true;
            Time.timeScale = 0;
            PauseUI.SetActive(true);
        }
    }

    //Win Condition
    public void Win()
    {
        WinLevelUI.SetActive(true);
        Time.timeScale = 0;
        isDisbaled = true;
        highscoretext.text = "Gulden terbanyak: " + highscore.ToString();
    }
    //Lose Condition
    public void Die()
    {
        GameOverUI.SetActive(true);
        Time.timeScale = 0;
        isDisbaled = true;
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

    #region scoring
    public void addScore(int Newscore)
    {
        //Update the existing score from the int
        score += Newscore;
        ScoreText.text = score.ToString() + " G";
        //Update the highscore if it's surpassed the highscore
        if (highscore < score)
        {
            PlayerPrefs.SetInt(currentlevel, score);
        }
    }
    public void MinusScore(int Newscore)
    {
        if(score > 0)
        {
            score -= Newscore;
            ScoreText.text = score.ToString() + " G";
        }
        else
        {
            return;
        }
    }
    #endregion
}
