using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthItem : MonoBehaviour
{
    #region Var
    PlayerHealth PlayerHealth;
    public int Health;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void giveHealth()
    {
        if (PlayerHealth.CurrentHealth < 
            PlayerHealth.MaxHealth)
        {
            int healthToAdd = Mathf.Min(
                Health, PlayerHealth.MaxHealth 
                - PlayerHealth.CurrentHealth);

            PlayerHealth.getHealth(healthToAdd);
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerHealth = other.gameObject.GetComponent<PlayerHealth>();

            gameObject.SetActive(false);
            PlayerAudio.instance.PlaySound("Health");
            giveHealth();
        }
    }
}
