using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyShooterState
{
    None,
    Idle,
    Patrol,
    Shoot
}

public class EnemyShooterScript : MonoBehaviour
{
    /* This script is made to set up the enemy movement and physics
     * Any mode changes will happen on the Finite State Machine
     * This script will detect if the condition of detecting met
     * and will communicated to IdleState -> Chase State -> etc.
     */

    #region Variable
    EnemyVAR var;
    public BulletPooling bullet;

    const string LEFT = "left";
    const string RIGHT = "right";

    private EnemyShooterState currentenemystate;
    public string CurrentStateName { get; private set; }

    string facingDir;

    public Transform Player;
    public Transform BulletPos;
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
        EnemyVAR.enAudio = GetComponentInChildren<EnemyAudio>();
    }

    private void Start()
    {
        //Default enemy direction
        baseScale = transform.localScale;
        facingDir = RIGHT;
        //Enemy State starting point
        currentenemystate = EnemyShooterState.Patrol;

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
        if (!var.isIdle)
        {
            detectPlayer();
        }

        //Every state functionality
        //Kondisi ketika enemy state idle
        if (currentenemystate == EnemyShooterState.Idle)
        {
            if (!var.isIdle)
            {
                currentenemystate = EnemyShooterState.Patrol;
            }
            else
            {
                rb.velocity = Vector2.zero;
                StartCoroutine(BacktoPatrol());
            }
        }
        //Kondisi ketika enemy state Patrol
        if (currentenemystate == EnemyShooterState.Patrol)
        {
            if (var.isIdle)
            {
                currentenemystate = EnemyShooterState.Idle;
            }
            else if (var.isDetectingPlayer)
            {
                //Ketika booleannya sudah true dia bakalan mainin kondisinya
                currentenemystate = EnemyShooterState.Shoot;
            }
            else
            {
                PatrolMovement(); //NOTE: Ketika menemukan obstacle atau jurang, akan balik ke idle
                ChangeDirRandom();
            }
        }
        if(currentenemystate == EnemyShooterState.Shoot)
        {
            if (!var.isDetectingPlayer)
            {
                currentenemystate = EnemyShooterState.Idle; var.isIdle = true;
            }
            else
            {
                AimAtPlayer();
            }
        }
    }

    private IEnumerator BacktoPatrol()
    {
        yield return new WaitForSeconds(1f);
        var.isIdle = false;
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
            ChangeFacingDirection((facingDir == LEFT) ? RIGHT : LEFT); //Flip the sprite
            var.isIdle = true;
        }
    }

    float _changeDirectionCooldown;
    private void ChangeDirRandom()
    {
        _changeDirectionCooldown -= Time.deltaTime;

        if (_changeDirectionCooldown <= 0)
        {
            ChangeFacingDirection((facingDir == LEFT) ? RIGHT : LEFT); //Flip the sprite
            _changeDirectionCooldown = Random.Range(1f, 10f);
        }
    }
    #endregion

    #region Attack Method
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
    public void ShootPlayer()
    {
        Debug.Log("Shooting Player");

        GameObject ammo = bullet.GetObject();
        Vector2 direction = new Vector2(this.transform.transform.localScale.x, 0);
        if(ammo == null)
        {
            return;
        }
        ammo.transform.position = BulletPos.position;
        ammo.gameObject.SetActive(true);
        ammo.GetComponent<ProjectileMove>().DirectionSetup(direction);
        EnemyVAR.enAudio.PlaySound("Shooting");
    }
    #endregion

    #region Aim Player
    private void AimAtPlayer()
    {
        //When the player position is on the right side of the enemy
        if (transform.position.x < Player.position.x)
        {
            //move to the right side
            ChangeFacingDirection(RIGHT);
        }
        //When the player position is on the right side of the enemy
        else if (transform.position.x > Player.position.x)
        {
            //move to the left side
            ChangeFacingDirection(LEFT);
        }
    }
    #endregion
}
