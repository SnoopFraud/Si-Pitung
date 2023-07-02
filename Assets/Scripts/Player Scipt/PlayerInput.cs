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
    [SerializeField] private CapsuleCollider regularcol, slidecol;
    [SerializeField] private Transform player;

    [Header("Physic Force")]
    public float _moveSpeed, jumpForce, crouchSpeed;
    public float BaseSpeed;
    public float gravity;

    [Header("Dashing Variable")]
    [SerializeField] private float _dashingVelocity;
    [SerializeField] private float _dashingTime;
    [SerializeField] private float CooldownTime;
    // Trail renderer
    [SerializeField] private TrailRenderer trailRenderer;

    private Vector2 _dashingDir;

    [Header("Sliding Variable")]
    //Sliding var
    [SerializeField] private float _slidingVelocity;
    [SerializeField] private float _slidingTime;
    [SerializeField] private float _slideCooldownTime;
    private Vector2 _slidingDir;
    
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

    [Header("Attack Variables")]
    public float AttackCooldownTimer;
    public float AttackSpeed1;
    public float AttackSpeed2;
    public float AttackSpeed3;
    public float PowerUpTime;

    public static PlayerInput instance;
    public bool isPowerUp;

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

        //Resetting var
        ResetFlag();
    }

    private void Update()
    {
        //Tidak bisa melakukan flip apabila sedang terjadi kondisi ini
        if (!PlayerVar.isDashing && !PlayerVar.isHitting[0] && !PlayerVar.isHitting[1] && !PlayerVar.isHitting[2])
        {
            FlipCharacter();
        }

        if(PlayerVar.isHitting[0] || PlayerVar.isHitting[1] || PlayerVar.isHitting[2])
        {
            _moveSpeed = 0;
        }
        else
        {
            _moveSpeed = BaseSpeed;
        }
    }

    private void FixedUpdate()
    {
        isPowerUp = PlayerVar.PowerUp;

        //For Knockback
        if (KnockbackCounter <= 0)
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

        //Condition for Sliding
        if(IsGrounded() && rb.velocity.magnitude > 0.1 && !PlayerVar.isSlidingCooldown)
        {
            PlayerVar.canSlide = true;
        }

        //For Sliding
        if (PlayerVar.isSliding)
        {
            rb.velocity = _slidingDir.normalized * _slidingVelocity;
            return; //Return to previous function
        }
        
        //For Dashing
        if (PlayerVar.isDashing)
        {
            rb.velocity = _dashingDir.normalized * _dashingVelocity; //Perform the dash when the condition is true
            return; //Return to previous function
        }

        //Condition for dashing
        if (IsGrounded() && !PlayerVar.isOnCooldown && rb.velocity.magnitude > 0.1)
        {
            PlayerVar.CanDash = true;
        }

        //Condition for Attack
        if(IsGrounded() && !PlayerVar.isAttackCooldown && !PlayerVar.isSliding)
        {
            PlayerVar.canAttack = true;
        }
    }

    private IEnumerator AttackCooldown()
    {
        PlayerVar.isAttackCooldown = true;
        yield return new WaitForSeconds(AttackCooldownTimer);
        PlayerVar.isAttackCooldown = false;
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

    private IEnumerator StopSliding()
    {
        yield return new WaitForSeconds(_slidingTime);
        //False the condition
        PlayerVar.isSliding = false;
        regularcol.enabled = true;
        slidecol.enabled = false;

        //Cooldown time
        PlayerVar.isSlidingCooldown = true;
        yield return new WaitForSeconds(_slideCooldownTime);
        PlayerVar.isSlidingCooldown = false;
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
        }
        if (PlayerVar.isFacingRight && horizontalMove < 0f)
        {
            Flip();
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
        PlayerAudio.instance.PlaySound("Jump");

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
            yield return new WaitForSeconds(0.3f);
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

        if (move.performed && IsGrounded())
        {
            PlayerAudio.instance.PlaySound("Footstep");
        }
        else
        {
            PlayerAudio.instance.StopSound("Footstep");
        }
    }

    public void Jump(InputAction.CallbackContext jump)
    {
        if (jump.performed && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            PlayerAudio.instance.PlaySound("Jump");
            PlayerAudio.instance.StopSound("Footstep");
        }
        if (jump.canceled && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
    }

    public void AttackControl(InputAction.CallbackContext combat)
    {
        if (combat.performed && PlayerVar.canAttack)
        {
            PlayerVar.canAttack = false;
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
                StartCoroutine(MovePlayerForward(0.1f, AttackSpeed2));
                PlayerVar.isHitting[2] = true;
            } 
            else if (PlayerVar.isHitting[2])
            {
                StartCoroutine(MovePlayerForward(0.1f, AttackSpeed3));
                StartCoroutine(AttackCooldown());
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
            PlayerVar.isDashing = true; //This will allow to execute the dashing velocity method in update
            PlayerVar.CanDash = false;
            trailRenderer.emitting = true;
            Physics.IgnoreLayerCollision(PlayerLayer, enemyLayer, true);
            
            _dashingDir = new Vector2(horizontalMove, 0);
            
            PlayerAudio.instance.PlaySound("Wind Dash");

            //Give direction for dashing
            if (_dashingDir == Vector2.zero)
            {
                _dashingDir = new Vector2(transform.localScale.x, 0);
            }
            StartCoroutine(StopDashing());
        }
    }

    public void Slide(InputAction.CallbackContext Sliding)
    {
        if (Sliding.performed && PlayerVar.canSlide)
        {
            PlayerVar.isSliding = true;
            PlayerVar.canSlide = false;
            //Switch collision
            regularcol.enabled = false;
            slidecol.enabled = true;

            _slidingDir = new Vector2(horizontalMove, 0);

            PlayerAudio.instance.PlaySound("Wind Dash");

            //Give Direction for sliding
            if (_slidingDir == Vector2.zero)
            {
                _slidingDir = new Vector2(transform.localScale.x, 0);
            }

            StartCoroutine(StopSliding());
        }
    }

    public void Pause(InputAction.CallbackContext pause)
    {
        if (pause.performed)
        {
            GameManager.instance.PauseGame();
        }
    }
    #endregion

    #region PowerUp
    public void StartingPowerUp()
    {
        StartCoroutine(PowerUpNow());
    }

    public IEnumerator PowerUpNow()
    {
        ResetAttack();
        PlayerVar.PowerUp = true;
        yield return new WaitForSeconds(PowerUpTime);
        PlayerVar.PowerUp = false;
        PlayerAudio.instance.PlaySound("Power Down");
        ResetAttack();
    }

    private void ResetAttack()
    {
        PlayerVar.isHitting[0] = false;
        PlayerVar.isHitting[1] = false;
        PlayerVar.isHitting[2] = false;
    }
    private void ResetFlag()
    {
        //Reset Attack
        PlayerVar.isHitting[0] = false;
        PlayerVar.isHitting[1] = false;
        PlayerVar.isHitting[2] = false;
        PlayerVar.isAttackCooldown = false;
        //Reset Sliding
        PlayerVar.isSliding = false;
        //Reset Dashing
        PlayerVar.isDashing = false;
        PlayerVar.isOnCooldown = false;
        //Reset Power Up
        PlayerVar.PowerUp = false;
        //Reenable collision
        Physics.IgnoreLayerCollision(regularcol.gameObject.layer, enemyLayer, false);
    }
    #endregion
}
