using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerHitbox : MonoBehaviour
{
    EnemyHealth enemyHealth;

    private void Awake()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            enemyHealth = other.gameObject.GetComponent<EnemyHealth>();

            if (PlayerVar.isHitting[0])
            {
                enemyHealth.TakeDamage(5);
            }
            if (PlayerVar.isHitting[1])
            {
                enemyHealth.TakeDamage(15);
            }
            if (PlayerVar.isHitting[2])
            {
                enemyHealth.TakeDamage(20);
            }
        }
    }
}
