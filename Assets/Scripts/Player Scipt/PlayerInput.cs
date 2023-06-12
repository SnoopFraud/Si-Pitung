using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private CapsuleCollider regularcol, crouchcol;
    [SerializeField] private GameObject hitbox;

    [Header("Physic Force")]
    public float _moveSpeed, jumpForce, crouchSpeed;
    public float BaseSpeed;

    [Header("Ground Check")]
    [SerializeField] private LayerMask WhatIsGround;
    [SerializeField] private Transform groundPoint;

    private float horizontalMove;

    public static PlayerInput instance;

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
        if (PlayerVar.crouching)
        {
            rb.velocity = new Vector2(horizontalMove * crouchSpeed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(horizontalMove * _moveSpeed, rb.velocity.y);
        }
    }

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
            PlayerVar.crouching = true;
            regularcol.enabled = false;
            crouchcol.enabled = true;
        }
        else
        {
            PlayerVar.crouching = false;
            regularcol.enabled = true;
            crouchcol.enabled = false;
        }
    }

    public void AttackControl(InputAction.CallbackContext combat)
    {
        if (combat.performed && !PlayerVar.onAtkCooldown && IsGrounded())
        {
            CombatInput.instance.ComboAttack();
        }
    }

    public void DashMovement(InputAction.CallbackContext dash)
    {
        if (PlayerVar.CanDash && rb.velocity.magnitude > 0)
        {
            StartCoroutine(CombatInput.instance.PerformDash());
            StartCoroutine(CombatInput.instance.MovePlayerForward(0.5f, 10f));
        }
    }
}
