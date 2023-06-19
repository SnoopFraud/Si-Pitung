using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public Rigidbody rb;
    public SpriteRenderer sr;

    //Set Player Value
    public float _moveSpeed, _jumpForce, _slideForce;
    //Set Direction
    private Vector2 moveDir;

    //Collider
    public CapsuleCollider regularcol;
    public CapsuleCollider slidecol;

    //Check ground
    public LayerMask WhatIsGround;
    public Transform _groundPoint;
    private bool isGrounded;

    //Animator
    public Animator anim;
    public Animator flip_anim; //For flipping

    private void Update()
    {
        moveDir.x = Input.GetAxis("Horizontal");
        moveDir.y = Input.GetAxis("Vertical");
        moveDir.Normalize();

        //3D movement
        //rb.velocity = new Vector3(moveDir.x * _moveSpeed, rb.velocity.y, moveDir.y * _moveSpeed);

        //2D Movement
        //transform.position += new Vector3(moveDir.x * _moveSpeed * Time.deltaTime, rb.velocity.y, 0);
        rb.velocity = new Vector2(moveDir.x * _moveSpeed, rb.velocity.y);
        anim.SetFloat("moveSpeed", rb.velocity.magnitude); //Set walking Speed

        RaycastHit hit;
        if (Physics.Raycast(_groundPoint.position, Vector3.down, out hit, .3f, WhatIsGround))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity += new Vector3(0f, _jumpForce, 0f);
        }
        anim.SetBool("onGround", isGrounded); //Set jump Animation
        
        //Ketika player sliding
        if (Input.GetKeyDown(KeyCode.LeftShift) && isGrounded)
        {
            //Do Slide
            DoSlide();
            //rb.velocity += new Vector3(moveDir.x * _slideForce , rb.velocity.y, 0);
        }

        //Flip the sprite to other direction
        if (!sr.flipX && moveDir.x < 0)
        {
            sr.flipX = true;
            flip_anim.SetTrigger("Flip");
        } 
        else if(sr.flipX && moveDir.x > 0)
        {
            sr.flipX = false;
            flip_anim.SetTrigger("Flip");
        }

    }

    void DoSlide()
    {
        //Activate Condition
        //isSlide = true;
        //Do Slide Animation
        anim.SetBool("Sliding", true);
        //Turn on Collider
        regularcol.enabled = false;
        slidecol.enabled = true;     
        //Start limit
        StartCoroutine("StopSlide");
    }

    IEnumerator StopSlide()
    {
        //Start countdowm
        yield return new WaitForSeconds(0.8f);
        //isSlide = false;
        slidecol.enabled = false;
        regularcol.enabled = true;
        anim.SetBool("Sliding", false);
    }

    //Reference
    /*
     * Core Movement: https://youtu.be/6obBCWLH1GI
     */
}
