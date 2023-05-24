using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthItem : MonoBehaviour
{
    #region Var
    GameObject PlayerHealth;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        PlayerHealth = GameObject.Find("/Player2.0");
    }

    void giveHealth()
    {
        if (PlayerHealth.GetComponent<PlayerHealth>().CurrentHealth < 
            PlayerHealth.GetComponent<PlayerHealth>().MaxHealth)
        {
            PlayerHealth.GetComponent<PlayerHealth>().getHealth(10);
        }
        else
        {
            Debug.Log("Health is Full");
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Score is " + ScoreUI.name);
    }

    private void OnCollisionEnter(Collision contactwith)
    {
        if (contactwith.gameObject.tag == "Player")
        {
            gameObject.SetActive(false);
            giveHealth();
        }
    }
}
