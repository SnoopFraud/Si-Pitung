using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_AnimController : MonoBehaviour
{
    //public static Enemy_AnimController instance;
    public EnemyScript enemy;

    private void Awake()
    {
        
    }

    public string[] AnimationName;
    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        if (!enemy.isIdle)
        {
            anim.SetInteger("State", 1);
        }
        if (enemy.isIdle)
        {
            anim.SetInteger("State", 0);
        }
        if (enemy.isDetectingPlayer)
        {
            anim.SetInteger("State", 2);
        }
        if (enemy.isAttackingPlayer)
        {
            anim.SetInteger("State", 3);
        }
    }
}
