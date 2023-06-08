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

    public override States RunCurrentState()
    {
        if (EnemyScript.instance.isDetectingPlayer)
        {
            return chaseState;
        }
        else
        {
            EnemyScript.instance.StopChase(); // 1. Idle animation transition to patrol
            // 2. Then do some patrol
            return this;
        }
    }
}
