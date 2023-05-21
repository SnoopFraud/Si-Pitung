using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    #region Var
    int score = 10;
    GameObject ScoreUI;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        ScoreUI = GameObject.Find("/Canvas/Scoring");    
    }

    public void GiveScore()
    {
        ScoreUI.GetComponent<Scoring>().AddScore(10);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Score is " + ScoreUI.name);
    }

    private void OnCollisionEnter(Collision contactwith)
    {
        if(contactwith.gameObject.tag == "Player")
        {
            gameObject.SetActive(false);
            GiveScore();
        }
    }
}
