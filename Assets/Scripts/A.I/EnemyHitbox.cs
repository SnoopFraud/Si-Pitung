using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitbox : MonoBehaviour
{
    PlayerHealth playerHealth;
    PlayerInput PlayerInput;

    private void Awake()
    {
        playerHealth = FindObjectOfType<PlayerHealth>();
        PlayerInput = FindObjectOfType<PlayerInput>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            PlayerInput.KnockbackCounter = PlayerInput.KnockbackTime;
            if(other.transform.position.x <= transform.position.x)
            {
                PlayerInput.KnockFromRight = true;
            }
            if (other.transform.position.x > transform.position.x)
            {
                PlayerInput.KnockFromRight = false;
            }
            playerHealth.takeDamage(10);
            StartCoroutine(PlayerInput.BlinkingEffect());
        }
    }
}
