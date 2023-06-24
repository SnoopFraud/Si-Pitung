using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    public static PlayerAnim instance;
    public Animator Animation;
    public string DashAnim;
    private string Attack1 = "Pitung_Attack1";
    private string Attack2 = "Pitung_Attack2";
    private string Attack3 = "Pitung_Attack3";

    private Rigidbody rb;

    private void Awake()
    {
        instance = this;
        rb = GetComponent<Rigidbody>();
    }

    private void nextHit()
    {
        for(int i = 0; i<PlayerVar.isHitting.Length - 1; i++)
        {
            if (Animation.GetCurrentAnimatorStateInfo(0).IsName("Pitung_Attack" + (i+1).ToString()) && 
                Animation.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            {
                PlayerVar.isHitting[i] = false;
                if (PlayerVar.isHitting[i+1])
                {
                    AnimationPlay("Pitung_Attack" + (i+2).ToString());
                }
            }
        }
    }

    private void Update()
    {
        BasicMovement();
        nextHit();

        if(Animation.GetCurrentAnimatorStateInfo(0).IsName(Attack3) &&
                Animation.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1) //When animation is finished
        {
            PlayerVar.isHitting[2] = false;
        }

        if(PlayerVar.isHitting[0] && !PlayerVar.isHitting[1] && !PlayerVar.isHitting[2])
        {
            Animation.Play(Attack1);
        }
    }

    void BasicMovement()
    {
        //Jump Animation
        Animation.SetBool("onGround", PlayerInput.instance.IsGrounded());
        //Crouch Animation
        Animation.SetBool("Crouch", PlayerVar.crouching);
        //Set the Dashing thing
        Animation.SetBool("Dashing", PlayerVar.isDashing);
        //Set the Speed
        Animation.SetFloat("moveSpeed", rb.velocity.magnitude);
    }

    public void AnimationPlay(string name)
    {
        Animation.Play(name);
    }
}
