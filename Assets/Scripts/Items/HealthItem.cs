using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthItem : MonoBehaviour
{
    #region Var
    GameObject PlayerHealth;
    PlayerHealth plhealth;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        PlayerHealth = GameObject.Find("/Player2.0");
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

            if(plhealth.CurrentHealth >= plhealth.MaxHealth)
            {
                PlayerHealth.GetComponent<PlayerHealth>().getHealth(0);
            }
            else
            {
                PlayerHealth.GetComponent<PlayerHealth>().getHealth(10);
            }
        }
    }
}
