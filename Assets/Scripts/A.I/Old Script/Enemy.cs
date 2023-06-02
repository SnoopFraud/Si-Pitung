using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum EnemyState
    {
        Patrol,
        Chase,
        Attack
    }

    private EnemyState currentState;
    //Var
    //Health
    [Header("Health")]
    public int maxHealth = 100;
    int currenthealth;
    //Speed
    [SerializeField]
    float moveSpeed;
    //Chase Range
    public float chaseRange = 5;
    //AI
    private Transform target;
    Rigidbody rb;


    // Start is called before the first frame update
    void Start()
    {
        currenthealth = maxHealth;
        rb = GetComponent<Rigidbody>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(transform.position, target.position);

        //Patrol state
        if (currentState == EnemyState.Patrol)
        {
            if (isFacingRight())
            {
                //Move right
                rb.velocity = new Vector2(moveSpeed, 0f);
            }
            else
            {
                rb.velocity = new Vector2(-moveSpeed, 0f);
            }
            //Detect Player within range
            if(distance < chaseRange)
            {
                currentState = EnemyState.Chase;
            }
        } 
        //Patrol
        //Chasing State
        if(currentState == EnemyState.Chase)
        {
            //Go to player
            if(target.position.x > transform.position.x)
            {
                //Move right
                rb.velocity = new Vector3(moveSpeed, 0);
            }
            else if(target.position.x < transform.position.x)
            {
                //Move left
                rb.velocity = new Vector3(-moveSpeed, 0);
            }
            //Detect Player is out of range
            if (distance < chaseRange)
            {
                currentState = EnemyState.Patrol;
            }
        }
    }

    bool isFacingRight()
    {
        if (transform.localScale.x > Mathf.Epsilon)
            return true;
        else
            return false;
    }

    public void takedamage(int damage)
    {
        currenthealth -= damage;

        //Play Hurt Animation

        if(currenthealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy Die");
        gameObject.SetActive(false);
    }

    private void OnTriggerExit(Collider other)
    {
        //Turn
        transform.localScale = new Vector2(-(Mathf.Sign(rb.velocity.x)), 1);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }
}
