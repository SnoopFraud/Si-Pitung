using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    # region variable
    [Header("Components")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private CapsuleCollider regularcol, crouchcol;
    [SerializeField] private GameObject hitbox;

    [Header("Physic Force")]
    public float _moveSpeed, _jumpForce, _crouchSpeed;
    public float BaseSpeed;
    public Vector2 movedir; [HideInInspector]

    [Header("Ground Check")]
    [SerializeField] private LayerMask WhatIsGround;
    [SerializeField] private Transform _groundPoint;

    public static PlayerInput instance;

    //To see where the character heading in the XY Axis
    
    [HideInInspector] public float _horizontalmove;
    # endregion

    #region Conditions
    public bool isGrounded()
    {
        RaycastHit hit;
        return Physics.Raycast(_groundPoint.position, Vector3.down, out hit, .3f, WhatIsGround);
    }

    void flip()
    {
        //Do Flip
        PlayerVar.isFacingRight = !PlayerVar.isFacingRight;
        sr.flipX = !sr.flipX;
    }
    #endregion

    #region Player Update
    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        _moveSpeed = BaseSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        //Only flip when player is not attacking
        if (!PlayerVar.isAttacking)
        {
            //Flip character
            if (!PlayerVar.isFacingRight && _horizontalmove > 0f)
            {
                flip();
                //Flip the Hitbox
                hitbox.transform.localPosition = new Vector3(1, 0, 0);
            }
            if (PlayerVar.isFacingRight && _horizontalmove < 0f)
            {
                flip();
                //Flip the Hitbox
                hitbox.transform.localPosition = new Vector3(-1, 0, 0);
            }
        }
    }
    private void FixedUpdate()
    {
        if (PlayerVar.crouching)
        {
            rb.velocity = new Vector2(_horizontalmove * _crouchSpeed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(_horizontalmove * _moveSpeed, rb.velocity.y);
        }
    }
    #endregion

    #region Player Control
    //Don't forget to add the controls on the Player Input System
    public void MovePlayer(InputAction.CallbackContext move)
    {
        _horizontalmove = move.ReadValue<Vector2>().x;
    }

    public void jump(InputAction.CallbackContext jump)
    {
        if(jump.performed && isGrounded() && PlayerVar.crouching == false)
        {
            rb.velocity = new Vector2(rb.velocity.x, _jumpForce);
        }
        //Jump canceled
        if(jump.canceled && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
    }

    public void crouch(InputAction.CallbackContext crouch)
    {
        //Press crouch button
        if(crouch.performed && isGrounded())
        {
            PlayerVar.crouching = true;
            regularcol.enabled = false;
            crouchcol.enabled = true;
        }
        //Press crouch cancelled
        else
        {
            PlayerVar.crouching = false;
            regularcol.enabled = true;
            crouchcol.enabled = false;
        }
    }
    #endregion
}
