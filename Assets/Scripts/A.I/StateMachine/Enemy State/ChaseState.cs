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
    [SerializeField] private bool onAttackRange;

    public override States RunCurrentState()
    {
        if(!EnemyScript.instance.isDetectingPlayer)
        {
            return idleState;
        }
        else
        {
            EnemyScript.instance.chasePlayer();
            return this;
        }
    }
}
