using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVar : MonoBehaviour
{
    PlayerVar instance;

    //Attack State
    public static bool[] isHitting = new bool[3];
    public static bool isAttackCooldown;
    public static bool canAttack;

    //For Dashing
    public static bool CanDash;
    public static bool isDashing;
    public static bool isOnCooldown;

    //For Sliding
    public static bool isSliding;

    //Crouching
    public static bool crouching;

    //for Flipping
    public static bool isFacingRight = true;

    //Death Condition
    public static bool isDead;

    private void Awake()
    {
        instance = this;
    }
}
