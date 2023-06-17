using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : States
{
    public PatrolState patrolState;
    public IdleState idleState;
    private bool canAttack = true;
    public float attackCooldown = 2f;
    public bool isCooldownFinish = false;

    public override States RunCurrentState()
    {
        if (!EnemyScript.instance.isAttackingPlayer)
        {
            return patrolState;
        }
        else if(canAttack)
        {
            StartCoroutine(AttackCooldown());
        } 
        return this;
    }
    private IEnumerator AttackCooldown()
    {
        canAttack = false; //disable attack

        // Perform attack action here
        //EnemyScript.instance.PerformAttack();

        yield return new WaitForSeconds(attackCooldown);

        canAttack = true;
        EnemyScript.instance.isAttackingPlayer = false;
    }
}
