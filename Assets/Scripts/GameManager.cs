using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    //Gameobjects
    [SerializeField] private GameObject GameOverUI;
    [SerializeField] private GameObject WinLevelUI;

    private void Awake()
    {
        instance = this;
        Time.timeScale = 1;
    }

    //Do function here
    //Win Condition
    //Lose Condition
    public void Die()
    {
        GameOverUI.SetActive(true);
        Time.timeScale = 0;
    }
    public void Win()
    {
        WinLevelUI.SetActive(true);
        Time.timeScale = 0;
    }
}
