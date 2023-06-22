using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    PlayerHealth instance;
    PlayerInput playerInput;
    private void Awake()
    {
        instance = this;
    }

    public int MaxHealth = 100;
    public int CurrentHealth;

    public HealthScript healthbar;

    private void Start()
    {
        //Find the health script
        healthbar = GameObject.FindObjectOfType<HealthScript>();
        playerInput = GetComponent<PlayerInput>();

        CurrentHealth = MaxHealth;
        healthbar.SetMaxHealth(MaxHealth);
    }

    private void Update()
    {
        //update the current health to the health bar
        healthbar.SetHealth(CurrentHealth);
        //Death condition
        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            gameObject.SetActive(false);
            //return;
        }
        //Debug.Log(CurrentHealth);
    }

    public void takeDamage(int damage)
    {
        CurrentHealth -= damage;
        StartCoroutine(playerInput.BlinkingEffect());
    }

    public void getHealth(int health)
    {
        CurrentHealth += health;
    }
}
