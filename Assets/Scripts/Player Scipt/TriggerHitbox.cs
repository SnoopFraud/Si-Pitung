using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerHitbox : MonoBehaviour
{
    EnemyHealth enemyHealth;
    EnemyVAR enemy;
    Enemy_AnimController EnemyAnim;

    private void Awake()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            enemy = other.gameObject.GetComponent<EnemyVAR>();
            enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
            EnemyAnim = other.gameObject.GetComponent<Enemy_AnimController>();

            enemy.Immobilized = true;
            EnemyAnim.Animation.Play("EnemyHurt");

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
