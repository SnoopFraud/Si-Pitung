using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CombatInput : MonoBehaviour
{
    #region Variable
    public Animator Anim;
    
    public LayerMask EnemyLayers;
    public Transform atkpoint;
    
    public float atkrange = 0.5f;
    public int atkDMG = 25;

    public float atkRate = 5f;
    float nextatkTime = 0f;
    #endregion

    void start()
    {
        
    }
    private void Update()
    {
        
    }

    #region Combat
    public void attack(InputAction.CallbackContext combat)
    {
        if (Time.time >= nextatkTime)
        {
            //Perform attack
            if (combat.performed)
            {
                melee();
                nextatkTime = Time.time + 1f / atkRate;
            }
        }
    }

    public void melee()
    {
        //Play Animation
        Anim.SetTrigger("Attack");
        //Detect enemy in the attack range
        Collider[] hitenemies = Physics.OverlapSphere(atkpoint.position, atkrange, EnemyLayers);
        //Damage the enemy
        foreach (Collider enemy in hitenemies)
        {
            //Debug.Log("We hit" + enemy.name);
            enemy.GetComponent<EnemyAI>().TakeDMG(atkDMG);
        }
    }
    #endregion

    private void OnDrawGizmosSelected()
    {
        if (atkpoint == null)
            return;

        Gizmos.DrawWireSphere(atkpoint.position, atkrange);
    }
}
