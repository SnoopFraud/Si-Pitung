using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimEvent : MonoBehaviour
{
    private EnemyShooterScript enemyShooter;

    private void Awake()
    {
        enemyShooter = GetComponentInParent<EnemyShooterScript>();
    }

    public void Shooting()
    {
        enemyShooter.ShootPlayer();
    }
}
