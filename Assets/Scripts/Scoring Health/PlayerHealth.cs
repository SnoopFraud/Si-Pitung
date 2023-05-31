using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int MaxHealth = 100;
    public int CurrentHealth;

    bool isInvincible = false;
    [SerializeField]
    private float InvinsibilitySeconds;
    [SerializeField]
    private float InvisibilityDeltaTime;

    public HealthScript healthbar;

    private void Start()
    {
        //Find the health script
        healthbar = GameObject.FindObjectOfType<HealthScript>();

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

    void takeDamage(int damage)
    {
        CurrentHealth -= damage;
    }

    public void getHealth(int health)
    {
        CurrentHealth += health;
    }

    private IEnumerator TempInvulnerability()
    {
        //Make player invulnerable for a limited amount of time
        Debug.Log("Player is Invincible"); isInvincible = true;

        for(float i = 0; i<InvinsibilitySeconds; i += InvisibilityDeltaTime)
        {
            yield return new WaitForSeconds(InvinsibilitySeconds);
        }

        isInvincible = false; Debug.Log("Player is No Longer Invincible");
    }

    private void OnCollisionEnter(Collision collision)
    {
        //take damage ketika kontak dg enemy
        if (collision.gameObject.CompareTag("Enemy"))
        {
            takeDamage(20);
        }
    }

    

}
