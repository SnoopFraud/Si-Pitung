using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    #region Var
    [SerializeField] private int score;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
           
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
            GameManager.instance.addScore(score);
            gameObject.SetActive(false);
            PlayerAudio.instance.PlaySound("Coin");
        }
    }
}
