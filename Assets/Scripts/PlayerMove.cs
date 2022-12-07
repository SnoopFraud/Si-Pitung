using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    //var
    [Header("Components")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private CapsuleCollider regularcol, slidecol;
    [SerializeField] private TrailRenderer tr;


    [Header("Physic Force")]
    public float _moveSpeed, _jumpForce;
    private Vector2 _moveDir;

    //Sliding Var
    private bool canSlide = true;
    private bool isSlide;
    private float slidePower = 24f;
    private float slideTime = 0.3f;
    private float slideCooldown = 1f;

    [Header("Ground Check")]
    [SerializeField] private LayerMask WhatIsGround;
    [SerializeField] private Transform _groundPoint;
    private bool isGrounded;

    [Header("Animation")]
    public Animator anim;
    public Animator flip_anim; //For flipping

    private void Update()
    {
        //Preventing player to do anything else while sliding
        if (isSlide)
        {
            return;
        }

        //References control input
        _moveDir.x = Input.GetAxisRaw("Horizontal");

        //Check isGround
        isitGround();

        //Do Jump
        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, _jumpForce);
        }
        //Do Jump higher
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
        //Set jump Animation
        anim.SetBool("onGround", isGrounded);
        //Do Slide
        if(Input.GetKeyDown(KeyCode.LeftShift) && canSlide && isGrounded && rb.velocity.magnitude > 0)
        {
            StartCoroutine(Sliding());
        }

        //Do Flip
        flip();
    }

    private void FixedUpdate()
    {
        if (isSlide)
        {
            return;
        }
        rb.velocity = new Vector2(_moveDir.x * _moveSpeed, rb.velocity.y);
        anim.SetFloat("moveSpeed", rb.velocity.magnitude); //Set walking Speed
    }

    //Check Ground
    private void isitGround()
    {
        RaycastHit hit;
        if (Physics.Raycast(_groundPoint.position, Vector3.down, out hit, .3f, WhatIsGround))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    //Flip function
    private void flip()
    {
        //Flip the sprite to other direction
        if (!sr.flipX && _moveDir.x < 0)
        {
            sr.flipX = true;
            flip_anim.SetTrigger("Flip");
        }
        else if (sr.flipX && _moveDir.x > 0)
        {
            sr.flipX = false;
            flip_anim.SetTrigger("Flip");
        }
    }

    //Slide Function
    private IEnumerator Sliding()
    {
        canSlide = false;
        isSlide = true;
        rb.velocity = new Vector2(_moveDir.x * slidePower, 0f);
        regularcol.enabled = false;
        slidecol.enabled = true;
        anim.SetBool("Sliding", true);
        tr.emitting = true;
        yield return new WaitForSeconds(slideTime);
        tr.emitting = false;
        isSlide = false;
        regularcol.enabled = true;
        slidecol.enabled = false;
        anim.SetBool("Sliding", false);
        yield return new WaitForSeconds(slideCooldown);
        canSlide = true;
    }
}
