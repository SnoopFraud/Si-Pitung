using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            other.gameObject.transform.GetComponent<PlayerInput>().StartingPowerUp();
            gameObject.SetActive(false);
            //ResetAttack();
        }
    }
}
