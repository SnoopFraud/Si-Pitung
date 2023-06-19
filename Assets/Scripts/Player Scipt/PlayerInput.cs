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
    [SerializeField] private Transform player;

    [Header("Physic Force")]
    public float _moveSpeed, jumpForce, crouchSpeed;
    public float BaseSpeed;
    public float gravity;

    [Header("Dashing Variable")]
    [SerializeField] private float _dashingVelocity;
    [SerializeField] private float _dashingTime;
    [SerializeField] private float CooldownTime;
    
    private Vector2 _dashingDir;
    // Trail renderer
    [SerializeField] private TrailRenderer trailRenderer;
    
    //For attack
    private Vector2 _attackDir;

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

    int PlayerLayer;
    int enemyLayer;
    #endregion

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        //Get Component
        trailRenderer = GetComponent<TrailRenderer>();

        PlayerLayer = LayerMask.NameToLayer("Player");
        enemyLayer = LayerMask.NameToLayer("Enemy");

        _moveSpeed = BaseSpeed;
    }

    private void Update()
    {
        FlipCharacter();
    }

    private void FixedUpdate()
    {
        if(KnockbackCounter <= 0)
        {
            //Apply gravity
            rb.AddForce(Vector2.down * gravity, ForceMode.Acceleration);
            
            //Movement speed calculation
            Vector2 movement = new Vector2(horizontalMove * _moveSpeed, rb.velocity.y);
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

        if (PlayerVar.isDashing)
        {
            rb.velocity = _dashingDir.normalized * _dashingVelocity; //Perform the dash when the condition is true
            return; //Return to previous function
        }

        if (PlayerVar.isHitting[0])
        {
            //Stop the player
            rb.velocity = Vector2.zero;
            return;
        }
        else if (PlayerVar.isHitting[1])
        {
            //Stop the player
            rb.velocity = Vector2.zero;

            StartCoroutine(MovePlayerForward(0.1f, 1f));
            return;
        }
        else if (PlayerVar.isHitting[2])
        {
            //Stop the player
            rb.velocity = Vector2.zero;

            StartCoroutine(MovePlayerForward(0.1f, 2f));
            return;
        }

        if (IsGrounded() && !PlayerVar.isOnCooldown && rb.velocity.magnitude > 0.1)
        {
            PlayerVar.CanDash = true;
        }
    }

    private IEnumerator StopDashing()
    {
        yield return new WaitForSeconds(_dashingTime);
        trailRenderer.emitting = false;
        PlayerVar.isDashing = false;
        Physics.IgnoreLayerCollision(PlayerLayer, enemyLayer, false);
        
        //Cooldown time
        PlayerVar.isOnCooldown = true;
        yield return new WaitForSeconds(CooldownTime);
        PlayerVar.isOnCooldown = false;
    }

    #region Physics Condition
    public bool IsGrounded()
    {
        RaycastHit hit;
        return Physics.Raycast(groundPoint.position, Vector3.down, out hit, .3f, WhatIsGround);
    }

    private void FlipCharacter()
    {
        if (!PlayerVar.isDashing)
        {
            if (!PlayerVar.isFacingRight && horizontalMove > 0f)
            {
                Flip();
            }
            if (PlayerVar.isFacingRight && horizontalMove < 0f)
            {
                Flip();
            }
        }
    }

    public IEnumerator MovePlayerForward(float duration, float speed)
    {
        Vector2 direction = player.right * (PlayerVar.isFacingRight ? 1f : -1f);
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            rb.AddForce(Vector3.Lerp(Vector3.zero, direction * speed, t), ForceMode.VelocityChange);
            yield return null;
        }
    }

    private void Flip()
    {
        PlayerVar.isFacingRight = !PlayerVar.isFacingRight;
        if (!PlayerVar.isFacingRight)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        } 
        else if (PlayerVar.isFacingRight)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
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
        if (combat.performed && IsGrounded() && !PlayerVar.crouching)
        {
            if (!PlayerVar.isHitting[0] && !PlayerVar.isHitting[1] && !PlayerVar.isHitting[2])
            {
                PlayerVar.isHitting[0] = true;
                _attackDir = new Vector2(horizontalMove, 0);
            } 
            else if (PlayerVar.isHitting[0])
            {
                PlayerVar.isHitting[1] = true;
            }
            else if (PlayerVar.isHitting[1])
            {
                PlayerVar.isHitting[2] = true;
            }

            if (_attackDir == Vector2.zero)
            {
                _attackDir = new Vector2(transform.localScale.x, 0);
            }
        }
    }

    public void DashMovement(InputAction.CallbackContext dash)
    {
        if(dash.performed && PlayerVar.CanDash)
        {
            PlayerVar.isDashing = true;
            PlayerVar.CanDash = false;
            trailRenderer.emitting = true;
            Physics.IgnoreLayerCollision(PlayerLayer, enemyLayer, true);
            _dashingDir = new Vector2(horizontalMove, 0);

            if (_dashingDir == Vector2.zero)
            {
                _dashingDir = new Vector2(transform.localScale.x, 0);
            }
            StartCoroutine(StopDashing());
        }
    }
    #endregion
}
