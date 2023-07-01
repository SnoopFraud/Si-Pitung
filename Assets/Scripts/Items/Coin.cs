using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    #region Var
    [SerializeField] private int score;
    GameObject ScoreUI;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        ScoreUI = GameObject.Find("/Canvas/Player UI/Scoring");    
    }

    public void GiveScore()
    {
        ScoreUI.GetComponent<Scoring>().AddScore(score);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Score is " + ScoreUI.name);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            gameObject.SetActive(false);
            PlayerAudio.instance.PlaySound("Coin");
            GiveScore();
        }
    }
}
