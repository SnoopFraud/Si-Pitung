using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerHitbox : MonoBehaviour
{
    EnemyHealth enemyHealth;
    EnemyVAR enemy;
    Enemy_AnimController EnemyAnim;

    Obstacle_Script obstacle;

    [SerializeField] private int damage1;
    [SerializeField] private int damage2;
    [SerializeField] private int damage3;

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
                enemyHealth.TakeDamage(damage1);
            }
            if (PlayerVar.isHitting[1])
            {
                enemyHealth.TakeDamage(damage2);
            }
            if (PlayerVar.isHitting[2])
            {
                enemyHealth.TakeDamage(damage3);
            }
        }

        if(other.gameObject.tag == "Obstacle")
        {
            obstacle = other.gameObject.GetComponent<Obstacle_Script>();

            if (PlayerVar.isHitting[0])
            {
                obstacle.TakeDamage(damage1);
            }
            if (PlayerVar.isHitting[1])
            {
                obstacle.TakeDamage(damage2);
            }
            if (PlayerVar.isHitting[2])
            {
                obstacle.TakeDamage(damage3);
            }
        }
    }
}
