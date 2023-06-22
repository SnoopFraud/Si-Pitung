using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_AnimController : MonoBehaviour
{
    //public static Enemy_AnimController instance;
    public EnemyVAR enemy;

    private void Awake()
    {
        
    }

    public string AttackAnimName;
    public Animator Animation;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        if (!enemy.isIdle)
        {
            Animation.SetInteger("State", 1);
        }
        if (enemy.isIdle)
        {
            Animation.SetInteger("State", 0);
        }
        if (enemy.isDetectingPlayer)
        {
            Animation.SetInteger("State", 2);
        }
        if (enemy.inAttackRange)
        {
            Animation.SetInteger("State", 3);
        }
    }
}
