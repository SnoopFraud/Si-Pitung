using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : States
{
    public static ChaseState instance;
    private void Awake()
    {
        instance = this;
    }

    public AttackState AttackState;
    public IdleState idleState;
    public PatrolState patrolState;

    [SerializeField] private bool onAttackRange;

    public override States RunCurrentState()
    {
        if(!EnemyScript.instance.isDetectingPlayer)
        {
            return patrolState;
        }
        else if (EnemyScript.instance.isAttackingPlayer)
        {
            return AttackState;
        }
        else if(EnemyScript.instance.isIdle)
        {
            return idleState;
        }
        else
        {
            if (!EnemyScript.instance.isIdle)
            {
                EnemyScript.instance.AttackRangeDetection();
                EnemyScript.instance.chasePlayer();
            }
            else
            {
                return this;
            }
        }

        return this;
    }
}
