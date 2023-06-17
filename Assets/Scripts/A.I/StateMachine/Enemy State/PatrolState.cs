using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : States
{
    public IdleState IdleState;
    public ChaseState chaseState;

    public override States RunCurrentState()
    {
        if (EnemyScript.instance.isDetectingPlayer)
        {
            return chaseState;
        }
        else if (EnemyScript.instance.isIdle)
        {
            return IdleState;
        }
        else
        {
            //Do some patrol
            EnemyScript.instance.PatrolMovement();
            return this;
        }
    }
}
