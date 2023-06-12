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
    const string LEFT = "left";
    const string RIGHT = "right";

    string facingDir;

    public Transform _player;
    public Transform castPoint; //for enemy sight point
    [SerializeField] Transform castpos; //for checking the wall
    public string _playerName;
    [SerializeField] float basecastDistance;
    public float Detectrange;
    public float moveSpeed;
    public bool isDetectingPlayer;

    Vector2 baseScale;

    public Rigidbody rb;
    public Animator anim;
    #endregion

    private void Start()
    {
        baseScale = transform.localScale;
        facingDir = RIGHT;

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

    private void FixedUpdate()
    {
        
    }

    #region Player Detection
    //This will detect the player in sight of the enemy
    bool canSeePlayer(float distance)
    {
        var castDist = distance;

        if (facingDir == RIGHT)
        {
            castDist = distance;
        }
        else
        {
            castDist = -distance;
        }

        Vector3 endpos = castPoint.position + Vector3.right * castDist;
        RaycastHit hit;
        Debug.DrawLine(castPoint.position, endpos, Color.red); //This is not consequential, just for knowning raycast
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
    // Below are functions to set up player movement and physics
    public void chasePlayer()
    {
        /* NOTE: why right is transform.position < player transform.position
         * is because it moves along on the XY coordinate, left means negative while right means positive
         */
        float ChaseSpeed = moveSpeed * 2; //Calculate to a more faster speed when chasing the player

        //When the player position is on the right side of the enemy
        if (transform.position.x < _player.position.x)
        {
            //move to the right side
            rb.velocity = new Vector2(ChaseSpeed, rb.velocity.y);
            ChangeFacingDirection(RIGHT);
        }
        //When the player position is on the right side of the enemy
        else if (transform.position.x > _player.position.x)
        {
            //move to the left side
            rb.velocity = new Vector2(-ChaseSpeed, rb.velocity.y);
            ChangeFacingDirection(LEFT);
        }
        //Kedua kondisi tetap jalankan animasi enemy run
        anim.Play("EnemyRunFaster");
    }

    public void PatrolMovement()
    {
        float vK = moveSpeed;

        if (facingDir == LEFT)
        {
            vK = -moveSpeed;
        }
        rb.velocity = new Vector2(vK, rb.velocity.y);
        anim.Play("EnemyRun");

        if (isHittingTheWall() || isOnEdge())
        {
            if (facingDir == LEFT)
            {
                ChangeFacingDirection(RIGHT);
            }
            else if (facingDir == RIGHT)
            {
                ChangeFacingDirection(LEFT);
            }
            Debug.Log("Hitting wall");
        }
    }

    void ChangeFacingDirection(string newDir)
    {
        Vector2 newScale = baseScale;
        if(newDir == LEFT)
        {
            newScale = new Vector2(-1, 1);
        }
        else if(newDir == RIGHT)
        {
            newScale = baseScale;
        }

        transform.localScale = newScale;
        facingDir = newDir;
    }

    bool isHittingTheWall()
    {
        bool val = false;

        float castDist = basecastDistance;

        //Define the cast distance for left and right
        if(facingDir == LEFT)
        {
            castDist = -basecastDistance;
        }
        else
        {
            castDist = basecastDistance;
        }

        //determine the target destination based on cast distance
        Vector3 targetpos = castpos.position;
        targetpos.x += castDist;

        Debug.DrawLine(castpos.position, targetpos, Color.red);

        if(Physics.Linecast(castpos.position, targetpos, 1 << LayerMask.NameToLayer("Ground")))
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

        float castDist = basecastDistance;

        //determine the target destination based on cast distance
        Vector3 targetpos = castpos.position;
        targetpos.y -= castDist;

        Debug.DrawLine(castpos.position, targetpos, Color.red);

        if (Physics.Linecast(castpos.position, targetpos, 1 << LayerMask.NameToLayer("Ground")))
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

    #region Enemy Attack

    #endregion
}
