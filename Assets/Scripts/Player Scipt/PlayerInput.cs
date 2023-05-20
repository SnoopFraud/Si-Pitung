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
    public Vector2 movedir; [HideInInspector]

    [Header("Ground Check")]
    [SerializeField] private LayerMask WhatIsGround;
    [SerializeField] private Transform _groundPoint;

    [Header("Animation")]
    public Animator anim;
    public Animator flip_anim; //For flipping

    //To see where the character heading in the XY Axis
    private float _horizontalmove;

    //Flipping
    public bool isFacingRight = true;

    //Crouching
    private bool crouching;
    # endregion

    #region Conditions
    private bool isGrounded()
    {
        RaycastHit hit;
        return Physics.Raycast(_groundPoint.position, Vector3.down, out hit, .3f, WhatIsGround);
    }

    void flip()
    {
        //Do Flip
        isFacingRight = !isFacingRight;
        sr.flipX = !sr.flipX;
        flip_anim.SetTrigger("Flip");
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    #region Player Update
    // Update is called once per frame
    void Update()
    {
        //Flip character
        if(!isFacingRight && _horizontalmove > 0f)
        {
            flip();
            //Flip the Hitbox
            hitbox.transform.localPosition = new Vector3(1, 0, 0);
        }
        if (isFacingRight && _horizontalmove < 0f)
        {
            flip();
            //Flip the Hitbox
            hitbox.transform.localPosition = new Vector3(-1, 0, 0); 
        }
        //Jump Animation
        anim.SetBool("onGround", isGrounded());
        //Crouch Animation
        anim.SetBool("Crouch", crouching);
    }
    private void FixedUpdate()
    {
        //Rigidbody velocity calc here
        if (crouching)
        {
            rb.velocity = new Vector2(_horizontalmove * _crouchSpeed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(_horizontalmove * _moveSpeed, rb.velocity.y);
            anim.SetFloat("moveSpeed", rb.velocity.magnitude);
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
        if(jump.performed && isGrounded() && crouching == false)
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
            crouching = true;
            regularcol.enabled = false;
            crouchcol.enabled = true;
        }
        //Press crouch cancelled
        else
        {
            crouching = false;
            regularcol.enabled = true;
            crouchcol.enabled = false;
        }
        Debug.Log(crouching);
    }
    #endregion
}
