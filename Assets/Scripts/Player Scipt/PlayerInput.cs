using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    #region Variable
    [Header("Components")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private CapsuleCollider regularcol, crouchcol;
    [SerializeField] private GameObject hitbox;

    [Header("Physic Force")]
    public float _moveSpeed, jumpForce, crouchSpeed;
    public float BaseSpeed;
    public float gravity;

    [Header("Ground Check")]
    [SerializeField] private LayerMask WhatIsGround;
    [SerializeField] private Transform groundPoint;

    private float horizontalMove;

    //Knockback variables
    [Header("Knockback Variables")]
    public float KnockbackForce;
    public float KnockbackCounter;
    public float KnockbackTime;

    public bool KnockFromRight;

    public static PlayerInput instance;
    #endregion

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        _moveSpeed = BaseSpeed;
    }

    private void Update()
    {
        if (!PlayerVar.isAttacking && !PlayerVar.isDashing)
        {
            FlipCharacter();
        }
    }

    private void FixedUpdate()
    {
        if(KnockbackCounter <= 0)
        {
            //Apply gravity
            rb.AddForce(Vector2.down * gravity, ForceMode.Acceleration);
            
            //Movement speed calculation
            Vector3 movement = new Vector2(horizontalMove * _moveSpeed, rb.velocity.y);
            rb.velocity = movement;
        }
        else
        {
            //Do the knocback
            if (KnockFromRight)
            {
                rb.velocity = new Vector2(-KnockbackForce, KnockbackForce);
            }
            if (!KnockFromRight)
            {
                rb.velocity = new Vector2(KnockbackForce, KnockbackForce);
            }
            KnockbackCounter -= Time.deltaTime;
        }
    }

    #region Physics Condition
    public bool IsGrounded()
    {
        RaycastHit hit;
        return Physics.Raycast(groundPoint.position, Vector3.down, out hit, .3f, WhatIsGround);
    }

    private void FlipCharacter()
    {
        if (!PlayerVar.isFacingRight && horizontalMove > 0f)
        {
            Flip();
            hitbox.transform.localPosition = new Vector3(1, 0, 0);
        }
        if (PlayerVar.isFacingRight && horizontalMove < 0f)
        {
            Flip();
            hitbox.transform.localPosition = new Vector3(-1, 0, 0);
        }
    }

    private void Flip()
    {
        PlayerVar.isFacingRight = !PlayerVar.isFacingRight;
        sr.flipX = !sr.flipX;
    }
    #endregion

    #region Effects
    public IEnumerator BlinkingEffect()
    {
        // Get the enemy layer index
        int enemyLayer = LayerMask.NameToLayer("Enemy");

        // Ignore enemy layer temporarily
        Physics.IgnoreLayerCollision(regularcol.gameObject.layer, enemyLayer, true);


        // Blink effect - toggle the visibility of the player renderer
        for (int i = 0; i < 5; i++)
        {
            sr.enabled = !sr.enabled;
            yield return new WaitForSeconds(0.2f);
        }

        // Re-enable collision with the enemy layer
        Physics.IgnoreLayerCollision(regularcol.gameObject.layer, enemyLayer, false);
        sr.enabled = true;
    }
    #endregion

    #region Control Physics
    public void MovePlayer(InputAction.CallbackContext move)
    {
        horizontalMove = move.ReadValue<Vector2>().x;
    }

    public void Jump(InputAction.CallbackContext jump)
    {
        if (jump.performed && IsGrounded() && !PlayerVar.crouching)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        if (jump.canceled && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
    }

    public void Crouch(InputAction.CallbackContext crouch)
    {
        if (crouch.performed && IsGrounded())
        {
            _moveSpeed = crouchSpeed;
            PlayerVar.crouching = true;
            regularcol.enabled = false;
            crouchcol.enabled = true;
        }
        else
        {
            _moveSpeed = BaseSpeed;
            PlayerVar.crouching = false;
            regularcol.enabled = true;
            crouchcol.enabled = false;
        }
    }

    public void AttackControl(InputAction.CallbackContext combat)
    {
        if (combat.performed && !PlayerVar.onAtkCooldown && IsGrounded() && !PlayerVar.crouching)
        {
            CombatInput.instance.ComboAttack();
        }
    }

    public void DashMovement(InputAction.CallbackContext dash)
    {
        if (PlayerVar.CanDash && rb.velocity.magnitude > 0 && !PlayerVar.crouching)
        {
            StartCoroutine(CombatInput.instance.PerformDash());
            StartCoroutine(CombatInput.instance.MovePlayerForward(0.5f, 10f));
        }
    }
    #endregion
}
