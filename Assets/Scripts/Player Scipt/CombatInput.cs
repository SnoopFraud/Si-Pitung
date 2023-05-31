using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum ComboState
{
    None,
    Attack1,
    Attack2,
    Attack3,
    AirAttack
}

public class CombatInput : MonoBehaviour
{
    #region Variable
    public static CombatInput instance;

    public LayerMask EnemyLayers;
    public Transform atkpoint;

    public float atkrange = 0.5f;

    //to Push
    public Transform Player;
    private Rigidbody rb;

    //Combo stuffs
    private ComboState currentComboState;
    private float currentComboTimer;
    private float defaultComboTime = 0.5f;
    private bool activateReseter;
    private float attackCooldown = 1f;
    #endregion

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        //For Combo Starter
        currentComboTimer = defaultComboTime;
        currentComboState = ComboState.None;
        PlayerVar.isAttacking = false;
        PlayerVar.onAtkCooldown = false;
    }

    private void Update()
    {
        ResetCombo();
    }

    #region Combat
    public void AttackControl(InputAction.CallbackContext combat)
    {
        //Every time the button pressed logic, don't block it using ComboState.None
        if (combat.performed)
        {
            if(!PlayerVar.onAtkCooldown)
            //Do the attack
            //every press will continue the chain until attack 3
            ComboAttack();
        }

    }

    void ComboAttack()
    {
        //Melakukan performa combo terakhir
        if (currentComboState == ComboState.Attack3)
        {
            return;
        }

        currentComboState++; //Adding enum after each combo
        PlayerVar.isAttacking = true;
        activateReseter = true; //Timer is on when combo is active
        currentComboTimer = defaultComboTime; //counting combo time from the default the combo start

        /* Each Combo will give different damage
         * Attack 1 = 5 DMG
         * Attack 2 = 10 DMG
         * Attack 3 = 15 DMG
         */

        switch (currentComboState)
        {
            /*Each case attack process
             * 1. do the attack (damage, play animation)
             * 2. Attack 1 stops player to move
             * 3. Attack 3 will activate the cooldown
             */
            case ComboState.Attack1:
                doAttack(5, "Player_Attack1");
                StartCoroutine("StopPlayerSpeed");
                break;
            case ComboState.Attack2:
                doAttack(10, "Player_Attack2");
                //Move the player
                StartCoroutine(MovePlayerForward(0.1f, 3f));
                break;
            case ComboState.Attack3:
                doAttack(15, "Player_Attack3");
                //Move the player further
                StartCoroutine(MovePlayerForward(0.2f, 5f));
                //Do Attack cooldown
                StartCoroutine("AttackCooldown");
                break;
        }
    }

    private void ResetCombo()
    {
        //Logic when the combo timer is active
        if (activateReseter)
        {
            currentComboTimer -= Time.deltaTime; //Timer doing a countdown until reset

            if(currentComboTimer <= 0) //The timer reaches 0
            {
                //Reset the combo back to square one
                currentComboState = ComboState.None;
                PlayerVar.isAttacking = false;
                activateReseter = false;
                currentComboTimer = defaultComboTime;
            }
        }
    }

    //Cooldown timer
    private IEnumerator AttackCooldown()
    {
        PlayerVar.onAtkCooldown = true;
        yield return new WaitForSeconds(attackCooldown);
        PlayerVar.onAtkCooldown = false;
    }

    //Function to intiate attack
    private void doAttack(int DMG, string AnimName)
    {
        //Play Animation
        PlayerAnim.instance.Animation.Play(AnimName);
        //Gave enemy damage
        HitEnemies(DMG);
    }

    //Function to gave enemy the damage
    private void HitEnemies(int DMG)
    {
        //Detect enemy in the attack range
        Collider[] hitenemies = Physics.OverlapSphere(atkpoint.position, atkrange, EnemyLayers);
        //Every enemy that is on the attack range
        foreach (Collider enemy in hitenemies)
        {
            //Give enemy the damage
            enemy.GetComponent<EnemyAI>().TakeDMG(DMG);
        }
    }

    private IEnumerator StopPlayerSpeed()
    {
        //Stop the player
        PlayerInput.instance._moveSpeed = 0;
        yield return new WaitForSeconds(0.7f);
        //Return to proper speed after finishing the attack
        PlayerInput.instance._moveSpeed = PlayerInput.instance.BaseSpeed;
    }

    private IEnumerator MovePlayerForward(float duration, float speed)
    {
        Vector3 direction = Player.right * (PlayerVar.isFacingRight ? 1f : -1f); // Calculate the movement direction
        float elapsedTime = 0f; //calculate the time

        while(elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            rb.AddForce(Vector3.Lerp(Vector3.zero, direction * speed, t), ForceMode.VelocityChange);
            yield return null;
        }
    }
    #endregion

    //Cuman biar tau dimana posisi atk damagenya
    private void OnDrawGizmosSelected()
    {
        if (atkpoint == null)
            return;

        Gizmos.DrawWireSphere(atkpoint.position, atkrange);
    }
}

