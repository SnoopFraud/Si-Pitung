using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CombatInput : MonoBehaviour
{
    #region Variables
    public static CombatInput instance;

    [Header("Combat Variable")]
    public LayerMask enemyLayers;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    #endregion

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    #region Combat Actions
    /*private void HitEnemies(int damage)
    {
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyHealth>().TakeDamage(damage);
        }
    }*/

    #endregion

    // Draw the attack range for visualization purposes
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
