using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    #region Variable
    [SerializeField] private int maxhealth = 100;
    [SerializeField] private int currenthealth;

    #endregion

    private void Start()
    {
        currenthealth = maxhealth;
    }

    #region health
    public void TakeDamage(int damage)
    {
        currenthealth -= damage;
        Debug.Log("Enemy HP: " + currenthealth);
        //Die
        if(currenthealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy Die");
        gameObject.SetActive(false);
    }
    #endregion
}
