using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    public static PlayerAnim instance;
    public Animator Animation;

    private Rigidbody rb;

    private void Awake()
    {
        instance = this;
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        //Jump Animation
        Animation.SetBool("onGround", PlayerInput.instance.isGrounded());
        //Crouch Animation
        Animation.SetBool("Crouch", PlayerVar.crouching);
        //Set the Speed
        Animation.SetFloat("moveSpeed", rb.velocity.magnitude);
    }
}
