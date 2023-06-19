using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    private void Update()
    {
        
    }

    #region health
    public void TakeDamage(int damage)
    {
        currenthealth -= damage;
        
        //Die
        if(currenthealth <= 0)
        {
            Die();
        }
        else
        {
            Debug.Log("Enemy HP: " + currenthealth);
        }
    }

    void Die()
    {
        Debug.Log("Enemy Die");
        this.gameObject.SetActive(false);
    }
    #endregion
}
