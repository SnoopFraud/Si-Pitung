using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    Idle,
    Patrol,
    Chase,
    Attack
}

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
    const string LEFT = "left";
    const string RIGHT = "right";

    private EnemyState currentenemystate;
    public string CurrentStateName { get; private set; }

    string facingDir;

    public Transform Player;
    public string PlayerName;
    public float moveSpeed;

    [SerializeField] private Transform castPoint; //for enemy sight point
    [SerializeField] private Transform WallCheck; //for checking the wall
    [SerializeField] private float DistancetoWall;
    
    [HideInInspector] public bool isDetectingPlayer;
    public float DetectionRange;

    [HideInInspector] public bool isAttackingPlayer;
    [HideInInspector] public bool canAttack = true;
    public bool isIdle;
    public float AttackRange;
    public float attackCooldown = 2f;
    public bool isCooldownFinish = false;

    Vector2 baseScale;

    public Rigidbody rb;
    #endregion

    private void Start()
    {
        baseScale = transform.localScale;
        facingDir = RIGHT;
        currentenemystate = EnemyState.Patrol;

        //Find the player transform component automatically
        if (Player == null)
        {
            Player = GameObject.FindGameObjectWithTag(PlayerName).transform;
        }
    }

    private void Update()
    {
        //Enemy will detect the player that are within detection range
        detectPlayer();
    }

    private void FixedUpdate()
    {
        StateHandler();
    }

    #region State Handler
    private void StateHandler()
    {
        //Every state functionality
        if (currentenemystate == EnemyState.Idle)
        {
            isIdle = true;
            StartCoroutine(StoppingMovement());

            if (isDetectingPlayer)
            {
                currentenemystate = EnemyState.Chase;
            }
        }
        if (currentenemystate == EnemyState.Patrol)
        {
            PatrolMovement();

            if (isDetectingPlayer)
            {
                currentenemystate = EnemyState.Chase;
            }
            if (isIdle)
            {
                currentenemystate = EnemyState.Idle;
            }
        }
        if (currentenemystate == EnemyState.Chase)
        {
            chasePlayer();
            AttackRangeDetection();

            if (!isDetectingPlayer)
            {
                currentenemystate = EnemyState.Patrol;
            }
            if (isIdle)
            {
                currentenemystate = EnemyState.Idle;
            }
            if (isAttackingPlayer)
            {
                currentenemystate = EnemyState.Attack;
            }
        }
        if (currentenemystate == EnemyState.Attack)
        {
            if (canAttack)
            {
                StartCoroutine(AttackCooldown());
            }

            if (!isAttackingPlayer)
            {
                if (isDetectingPlayer)
                {
                    currentenemystate = EnemyState.Chase;
                }
                else if (!isDetectingPlayer)
                {
                    currentenemystate = EnemyState.Patrol;
                }
            }
        }
    }
    #endregion

    #region Player Detection
    //This will detect the player in sight of the enemy
    bool canSeePlayer(float distance)
    {
        float castDist = (facingDir == RIGHT) ? distance : -distance; //Flip the sight, more cleaner if else
        Vector3 endPos = castPoint.position + Vector3.right * castDist;
        Debug.DrawLine(castPoint.position, endPos, Color.red); //for Debug Draw Line

        return Physics.Linecast(castPoint.position, endPos, out RaycastHit hit, 1 << LayerMask.NameToLayer("Player"));
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
        bool isPlayerDetected = canSeePlayer(DetectionRange);
        isDetectingPlayer = isPlayerDetected;
    }
    #endregion

    #region Wall and Edge Condition
    bool isHittingTheWall()
    {
        bool val = false;

        //Define the cast distance for left and right
        float castDist = (facingDir == LEFT) ? -DistancetoWall : DistancetoWall;

        //determine the target destination based on cast distance
        Vector3 targetpos = WallCheck.position;
        targetpos.x += castDist;

        Debug.DrawLine(WallCheck.position, targetpos, Color.red);

        if (Physics.Linecast(WallCheck.position, targetpos, 1 << LayerMask.NameToLayer("Obstacle")))
        {
            val = true;
        }
        else
        {
            val = false;
        }

        return val;
    }
    bool isOnEdge()
    {
        bool val = false;

        float castDist = DistancetoWall;

        //determine the target destination based on cast distance
        Vector3 targetpos = WallCheck.position;
        targetpos.y -= castDist;

        Debug.DrawLine(WallCheck.position, targetpos, Color.red);

        if (Physics.Linecast(WallCheck.position, targetpos, 1 << LayerMask.NameToLayer("Ground")))
        {
            val = false;
        }
        else
        {
            val = true;
        }

        return val;
    }
    #endregion

    #region Flipiing the Player
    void ChangeFacingDirection(string newDir)
    {
        Vector2 newScale = (newDir == LEFT) ? new Vector2(-1, 1) : baseScale;
        transform.localScale = newScale;
        facingDir = newDir;
    }
    #endregion

    #region Chase Player
    // Below are functions to set up player movement and physics
    public void chasePlayer()
    {
        /* NOTE: why right is transform.position < player transform.position
         * is because it moves along on the XY coordinate, left means negative while right means positive
         */
        float ChaseSpeed = moveSpeed * 2; //Calculate to a more faster speed when chasing the player
        Vector2 targetVelocity;

        //When the player position is on the right side of the enemy
        if (transform.position.x < Player.position.x)
        {
            //move to the right side
            targetVelocity = new Vector2(ChaseSpeed, rb.velocity.y);
            ChangeFacingDirection(RIGHT);
        }
        //When the player position is on the right side of the enemy
        else if (transform.position.x > Player.position.x)
        {
            //move to the left side
            targetVelocity = new Vector2(-ChaseSpeed, rb.velocity.y);
            ChangeFacingDirection(LEFT);
        }
        else
        {
            // Player is at the same position, stop moving horizontally
            targetVelocity = new Vector2(0f, rb.velocity.y);
        }
        //Kedua kondisi tetap jalankan animasi enemy run
        rb.velocity = targetVelocity;
        //Play Animation
    }
    #endregion

    #region Enemy Patrol and Idle

    //For Patrol
    public void PatrolMovement()
    {
        //Untuk melakukan pergerakan patroli
        float horizontalSpeed = (facingDir == LEFT) ? -moveSpeed : moveSpeed;

        rb.velocity = new Vector2(horizontalSpeed, rb.velocity.y);
        //Play Animation

        if (isHittingTheWall() || isOnEdge())
        {
            currentenemystate = EnemyState.Idle;
            ChangeFacingDirection((facingDir == LEFT) ? RIGHT : LEFT); //Flip the sprite
        }
    }

    //for Idle
    public IEnumerator StoppingMovement()
    {
        rb.velocity = Vector2.zero;
        //Play Animation
        yield return new WaitForSeconds(2f);
        currentenemystate = EnemyState.Patrol;
        isIdle = false;
    }
    #endregion

    #region Attack
    bool isAttacking(float distance)
    {
        float castDist = (facingDir == RIGHT) ? distance : -distance; //Flip player, more cleaner if else
        Vector3 endPos = castPoint.position + Vector3.right * castDist;
        Debug.DrawLine(castPoint.position, endPos, Color.green); //for Debug Draw Line

        return Physics.Linecast(castPoint.position, endPos, out RaycastHit hit, 1 << LayerMask.NameToLayer("Player"));
    }
    public void AttackRangeDetection()
    {
        bool PlayerinAttackZone = isAttacking(AttackRange);
        isAttackingPlayer = PlayerinAttackZone;
    }
    public void PerformAttack()
    {
        rb.velocity = Vector2.zero;
        //Play Animation
        //Debug.Log("Is Attacking");
    }
    private IEnumerator AttackCooldown()
    {
        canAttack = false; //disable attack
        isCooldownFinish = false;

        // Perform attack action here
        PerformAttack();

        yield return new WaitForSeconds(attackCooldown);

        canAttack = true;
        isCooldownFinish = true;
        isAttackingPlayer = false;
    }
    #endregion
}
