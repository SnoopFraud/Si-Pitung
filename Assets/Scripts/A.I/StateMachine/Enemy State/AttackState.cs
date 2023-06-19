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
        return this;
    }
}
