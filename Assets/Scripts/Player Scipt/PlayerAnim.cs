using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    public static PlayerAnim instance;
    
    public Animator CurrentAnimator;

    public GameObject[] AnimationVar = new GameObject[2];

    private string Attack1 = "Pitung_Attack1";
    private string Attack2 = "Pitung_Attack2";
    private string Attack3 = "Pitung_Attack3";

    private Rigidbody rb;

    private void Awake()
    {
        instance = this;
        rb = GetComponent<Rigidbody>();

        CurrentAnimator = AnimationVar[0].GetComponentInChildren<Animator>();
    }

    private void nextHit()
    {
        for(int i = 0; i<PlayerVar.isHitting.Length - 1; i++)
        {
            if (CurrentAnimator.GetCurrentAnimatorStateInfo(0).IsName("Pitung_Attack" + (i+1).ToString()) && 
                CurrentAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
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

        if(CurrentAnimator.GetCurrentAnimatorStateInfo(0).IsName(Attack3) &&
                CurrentAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1) //When animation is finished
        {
            PlayerVar.isHitting[2] = false;
        }

        if(PlayerVar.isHitting[0] && !PlayerVar.isHitting[1] && !PlayerVar.isHitting[2])
        {
            CurrentAnimator.Play(Attack1);
        }

        if (PlayerVar.PowerUp)
        {
            //Switch the game object
            AnimationVar[0].SetActive(false);
            AnimationVar[1].SetActive(true);

            CurrentAnimator = AnimationVar[1].GetComponentInChildren<Animator>();
        }
        else
        {
            //Switch the game object
            AnimationVar[0].SetActive(true);
            AnimationVar[1].SetActive(false);

            CurrentAnimator = AnimationVar[0].GetComponentInChildren<Animator>();
        }
    }

    void BasicMovement()
    {
        //Jump Animation
        CurrentAnimator.SetBool("onGround", PlayerInput.instance.IsGrounded());
        //Crouch Animation
        CurrentAnimator.SetBool("Sliding", PlayerVar.isSliding);
        //Set the Dashing thing
        CurrentAnimator.SetBool("Dashing", PlayerVar.isDashing);
        //Set the Speed
        CurrentAnimator.SetFloat("moveSpeed", rb.velocity.magnitude);
    }

    public void AnimationPlay(string name)
    {
        CurrentAnimator.Play(name);
    }
}
