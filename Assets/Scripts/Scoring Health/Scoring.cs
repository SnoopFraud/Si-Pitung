using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Scoring : MonoBehaviour
{
    #region var
    public TextMeshProUGUI scoretxt;
    public int score = 0;
    public int MaxScore;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
    }

    public void AddScore(int newScore)
    {
        score += newScore;
    }

    public void UpdateScore()
    {
        scoretxt.text = "0" + score;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateScore();
    }
}
