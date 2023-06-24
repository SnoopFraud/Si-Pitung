using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    PlayerHealth instance;
    PlayerInput playerInput;

    public int MaxHealth = 100;
    public int CurrentHealth;

    public HealthScript healthbar;

    private void Awake()
    {
        instance = this;
        CurrentHealth = MaxHealth;
    }

    private void Start()
    {
        //Find the health script
        healthbar = GameObject.FindObjectOfType<HealthScript>();
        healthbar.SetMaxHealth(MaxHealth);
        playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        //update the current health to the health bar
        healthbar.SetHealth(CurrentHealth);
        //Death condition
        if (CurrentHealth <= 0)
        {
            //Die
            GameManager.instance.Die();
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

    public void BaseHealth()
    {
        CurrentHealth = MaxHealth;
    }
}
