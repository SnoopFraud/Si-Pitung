using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    #region Class instance
    public static EnemyScript instance;

    private void Awake()
    {
        instance = this;
    }
    #endregion

    /* This script is made to set up the enemy movement and physics
     * Any mode changes will happen on the Finite State Machine
     * This script will detect if the condition of detecting met
     * and will communicated to IdleState -> Chase State -> etc.
     */
    
    #region Variable
    public Transform _player;
    public Transform castPoint;
    public string _playerName;
    public float Detectrange;
    public float moveSpeed;
    public bool isDetectingPlayer;
    public bool isSearching;
    public bool isFacingRight;

    public Rigidbody rb;
    public Animator anim;
    #endregion

    private void Start()
    {
        //Get the rigidbody component
        rb = GetComponent<Rigidbody>();
        //Find the player transform component automatically
        if (_player == null)
        {
            _player = GameObject.FindGameObjectWithTag(_playerName).transform;
        }
    }

    private void Update()
    {
        //Enemy will detect the player that are within detection range
        //Other functionality is activated on Finite State Machine
        detectPlayer();
    }

    #region Player Detection
    //This will detect the player in sight of the enemy
    bool canSeePlayer(float distance)
    {
        var castDist = distance;

        if (isFacingRight == false)
        {
            castDist = -distance;
        }

        Vector3 endpos = castPoint.position + Vector3.right * castDist;
        RaycastHit hit;
        return Physics.Linecast(castPoint.position, endpos, out hit, 1 << LayerMask.NameToLayer("Player"));
    }

    //This is for detecting player
    public void detectPlayer()
    {
        /* If the player within the raycast of the enemy sight
         * isDetectingPlayer = true -> ChaseState
         * if outside the range isDetecting = false -> IdleState
         * 
         * Please do note that
         * 1. to ChasePlayer is activated by the ChaseState
         * 2. to StopChase is activated by the IdleState (It's mostly for animation)
         */
        if (canSeePlayer(Detectrange))
        {
            //Change state to chase the player
            isDetectingPlayer = true;
        }
        else
        {
            isDetectingPlayer = false;
        }
    }
    #endregion

    #region enemy movement
    public void chasePlayer()
    {
        /* NOTE: why right is transform.position < player transform.position
         * is because it moves along on the XY coordinate, left means negative while right means positive
         */
        
        //When the player position is on the right side of the enemy
        if (transform.position.x < _player.position.x)
        {
            //move to the right side
            rb.velocity = new Vector2(EnemyScript.instance.moveSpeed, 0);
            transform.localScale = new Vector2(1, 1);
            isFacingRight = true;
        }
        //When the player position is on the right side of the enemy
        else if (transform.position.x > _player.position.x)
        {
            //move to the left side
            rb.velocity = new Vector2(-EnemyScript.instance.moveSpeed, 0);
            transform.localScale = new Vector2(-1, 1);
            isFacingRight = false;
        }
        //Kedua kondisi tetap jalankan animasi enemy run
        anim.Play("EnemyRun");
    }

    public void StopChase()
    {
        rb.velocity = Vector2.zero;
        anim.Play("EnemyIdle");
    }
    #endregion
}
