using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    #region Variable
    public int maxhealth = 100;
    int currenthealth;

    public GameObject scoreui;

    #endregion

    private void Start()
    {
        currenthealth = maxhealth;
    }

    public void TakeDMG(int damage)
    {
        currenthealth -= damage;
        //Die
        if(currenthealth <= 0)
        {
            Die();
            GiveScore();
        }
    }

    public void GiveScore()
    {
        scoreui.GetComponent<Scoring>().AddScore(10);
    }

    void Die()
    {
        Debug.Log("Enemy Die");
        gameObject.SetActive(false);
    }
}
