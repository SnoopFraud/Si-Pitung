using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    None,
    Idle,
    Patrol,
    Chase,
    Attack
}

public class EnemyScript : MonoBehaviour
{
    /* This script is made to set up the enemy movement and physics
     * Any mode changes will happen on the Finite State Machine
     * This script will detect if the condition of detecting met
     * and will communicated to IdleState -> Chase State -> etc.
     */

    #region Variable
    EnemyVAR var;

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

    public float DetectionRange;
    public float AttackRange;

    private float gravity = 9.8f;

    Vector2 baseScale;

    public Rigidbody rb;
    #endregion

    private void Awake()
    {
        var = GetComponent<EnemyVAR>();
    }

    private void Start()
    {
        //Default enemy direction
        baseScale = transform.localScale; 
        facingDir = RIGHT;
        //Enemy State starting point
        currentenemystate = EnemyState.Patrol;

        //Find the player transform component automatically
        if (Player == null)
        {
            Player = GameObject.FindGameObjectWithTag(PlayerName).transform;
        }
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (!var.Immobilized)
        {
            //Apply gravity
            rb.AddForce(Vector2.down * gravity, ForceMode.Acceleration);
            StateHandler();
        }
        else
        {
            rb.velocity = Vector2.zero;

            //When the player position is on the right side of the enemy
            if (transform.position.x < Player.position.x)
            {
                ChangeFacingDirection(RIGHT);
            }
            //When the player position is on the right side of the enemy
            else if (transform.position.x > Player.position.x)
            {
                //move to the left side
                ChangeFacingDirection(LEFT);
            }

            StartCoroutine(MobilizedEnemy());
        }
        
    }

    private IEnumerator MobilizedEnemy()
    {
        yield return new WaitForSeconds(0.5f);
        var.Immobilized = false;
    }

    #region State Handler
    private void StateHandler()
    {
        if (!var.isIdle) //Ketika idle, enemy istirahat dari cari player
        {
            //Enemy will detect the player that are within detection range
            detectPlayer();
        }


        //Every state functionality
        //Kondisi ketika enemy state idle
        if (currentenemystate == EnemyState.Idle)
        {
            if (!var.isIdle)
            {
                currentenemystate = EnemyState.Patrol;
            }
            else
            {
                rb.velocity = Vector2.zero;
                StartCoroutine(BacktoPatrol());
            }
        }
        //Kondisi ketika enemy state Patrol
        if (currentenemystate == EnemyState.Patrol)
        {
            if (var.isIdle)
            {
                currentenemystate = EnemyState.Idle;
            }
            else if (var.isDetectingPlayer)
            {
                currentenemystate = EnemyState.Chase;
            }
            else
            {
                PatrolMovement(); //NOTE: Ketika menemukan obstacle atau jurang, akan balik ke idle
                ChangeDirRandom();
            }
        }
        //Kondisi ketika enemy state chase
        if(currentenemystate == EnemyState.Chase)
        {
            if (!var.isDetectingPlayer)
            {
                currentenemystate = EnemyState.Idle; var.isIdle = true;
            }
            else if (var.inAttackRange)
            {
                currentenemystate = EnemyState.Attack;
            }
            else
            {
                chasePlayer();
                AttackRangeDetection();
            }
        }
        //Kondisi ketika enemy state attack
        if(currentenemystate == EnemyState.Attack)
        {
            if (!var.inAttackRange)
            {
                currentenemystate = EnemyState.Chase;
            }
            else
            {
                rb.velocity = Vector2.zero;
                StartCoroutine(AttackReturn());
            }
        }
    }

    private IEnumerator BacktoPatrol()
    {
        yield return new WaitForSeconds(1f);
        var.isIdle = false;
    }

    private IEnumerator AttackReturn()
    {
        yield return new WaitForSeconds(2f);
        var.inAttackRange = false;
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
        var.isDetectingPlayer = isPlayerDetected;
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

        if (Physics.Linecast(WallCheck.position, targetpos, 1 << LayerMask.NameToLayer("Obstacle")) 
            || Physics.Linecast(WallCheck.position, targetpos, 1 << LayerMask.NameToLayer("Ground")))
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
        
        if (isHittingTheWall() || isOnEdge())
        {
            ChangeFacingDirection((facingDir == LEFT) ? RIGHT : LEFT); //Flip the sprite
            var.isIdle = true;
        }
    }

    float _changeDirectionCooldown;
    private void ChangeDirRandom()
    {
        _changeDirectionCooldown -= Time.deltaTime;

        if(_changeDirectionCooldown <= 0)
        {
            ChangeFacingDirection((facingDir == LEFT) ? RIGHT : LEFT); //Flip the sprite
            _changeDirectionCooldown = Random.Range(1f, 10f);
        }
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
        var.inAttackRange = PlayerinAttackZone;
    }
    #endregion
}
