using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int MaxHealth = 100;
    public int CurrentHealth;

    public HealthScript healthbar;

    private void Start()
    {
        healthbar = GameObject.FindObjectOfType<HealthScript>();

        CurrentHealth = MaxHealth;
        healthbar.SetMaxHealth(MaxHealth);
    }

    private void Update()
    {
        if (CurrentHealth <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    void takeDamage(int damage)
    {
        CurrentHealth -= damage;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            takeDamage(20);
            healthbar.SetHealth(CurrentHealth);
        }
    }

}
