using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVar : MonoBehaviour
{
    PlayerVar instance;

    //Attack State
    public static bool isAttacking;
    public static bool onAtkCooldown;

    //For Dashing
    public static bool CanDash;
    public static bool isDashing;

    //Crouching
    public static bool crouching;

    //for Flipping
    public static bool isFacingRight = true;

    private void Awake()
    {
        instance = this;
    }
}
