using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMove : MonoBehaviour
{
    ProjectileMove instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public float speed = 10f;
    public Vector2 direction;

    private void OnEnable()
    {
        Invoke("Deactivate", 2f);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void FixedUpdate()
    {
        //Move the bottle
        transform.Translate(direction * speed * Time.deltaTime);

        if(direction.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        } 
        else if (direction.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public void DirectionSetup(Vector2 dir)
    {
        direction = dir;
    }
}
