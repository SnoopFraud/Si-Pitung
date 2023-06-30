using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle_Script : MonoBehaviour
{
    [SerializeField] private int maxhealth = 50;
    [SerializeField] private int currenthealth;
    [SerializeField] private Transform ItemSpawnerLoc;

    [SerializeField] private ItemPooling itemPool;
    
    public Animator anim;

    private void Start()
    {
        currenthealth = maxhealth;
    }

    private void Update()
    {

    }

    #region health
    public void TakeDamage(int damage)
    {
        currenthealth -= damage;
        anim.Play("BarrelDMG");

        //Die
        if (currenthealth <= 0)
        {
            //Die
            gameObject.SetActive(false);
            //Spawn Object
            SpawnObject();
        }
        else
        {
            Debug.Log("Crate HP: " + currenthealth);
        }
    }

    private void SpawnObject()
    {
        //The Gameobject that are spawned are taken from the pooled object
        GameObject item = itemPool.GetObject();

        if (item == null)
        {
            return;
        }

        //Spawn the item on that position
        item.transform.position = ItemSpawnerLoc.transform.position;
        item.gameObject.SetActive(true);
    }
    #endregion
}
