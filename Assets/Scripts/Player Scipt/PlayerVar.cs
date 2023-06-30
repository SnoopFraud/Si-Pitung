using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVar : MonoBehaviour
{
    //Attack State
    public static bool[] isHitting = new bool[3];
    public static bool isAttackCooldown;
    public static bool canAttack;

    //For Dashing
    public static bool CanDash;
    public static bool isDashing;
    public static bool isOnCooldown;

    //For Sliding
    public static bool canSlide;
    public static bool isSliding;
    public static bool isSlidingCooldown;

    //for Flipping
    public static bool isFacingRight = true;

    //Death Condition
    public static bool isDead;

    //Getting a power up
    public static bool PowerUp;
}
