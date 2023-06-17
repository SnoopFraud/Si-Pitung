using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : States
{
    public static IdleState instance;
    private void Awake()
    {
        instance = this;
    }

    public ChaseState chaseState;
    public PatrolState patrolState;

    public override States RunCurrentState()
    {
        if (!EnemyScript.instance.isIdle)
        {
            return patrolState;
        }
        else if (EnemyScript.instance.isDetectingPlayer)
        {
            EnemyScript.instance.isIdle = false;
            return chaseState;
        }
        else
        {
            StartCoroutine(EnemyScript.instance.StoppingMovement());
            return this;
        }
    }
}
