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
    float lastAttackTime = 0f;

    public float pushForce = 50f;
    public Transform Player;
    private Rigidbody rb;
    #endregion

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        
    }

    #region Combat
    public void attack(InputAction.CallbackContext combat)
    {
        if (combat.performed && Time.time >= lastAttackTime + 5f / atkRate)
        {
            melee();
            lastAttackTime = Time.time; // Update the time of the last attack
        }
    }

    public void melee()
    {
        //Play Animation
        Anim.SetTrigger("Attack");
        //Put some force
        Vector3 pushDirection = Player.right; // Set the push direction based on the player's right direction
        if (!GetComponent<PlayerInput>().isFacingRight) // If the player is not facing right, flip the push direction
        {
            pushDirection *= -1f;
        }
        rb.AddForce(pushDirection * pushForce, ForceMode.Impulse);

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
