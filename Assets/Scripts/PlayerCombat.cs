using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Animator atkanim;
    public Transform atkpoint;
    public float atkrange = 0.5f;
    public LayerMask enemyLayer;

    public int atkDmg = 40;
    public float atkrate = 5f;
    float nextatktime = 0f;

    PlayerMove pl;

    private void Awake()
    {
        pl = GetComponent<PlayerMove>();
    }

    private void Update()
    {
        if (Time.time >= nextatktime)
        {
            if (Input.GetKeyDown(KeyCode.O))
            {
                attack();
                nextatktime = Time.time + 1f / atkrate;
            }
        }
    }

    void attack()
    {
        //Play attack animation
        atkanim.SetTrigger("Attack");

        //Detect the range
        Collider[] hitenemies = Physics.OverlapSphere(atkpoint.position, atkrange, enemyLayer);
        //Damage the enemy
        foreach(Collider enemy in hitenemies)
        {
            enemy.GetComponent<Enemy>().takedamage(atkDmg);   
        }
    }

    private void OnDrawGizmosSelected()
    {
        if(atkpoint == null)
        {
            return;
        }

        Gizmos.DrawWireSphere(atkpoint.position, atkrange);
    }
}
