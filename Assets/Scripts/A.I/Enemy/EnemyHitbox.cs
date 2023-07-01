using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class EnemyHitbox : MonoBehaviour
{
    PlayerHealth playerHealth;
    PlayerInput PlayerInput;

    public int damage;

    private void Awake()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            playerHealth = other.gameObject.GetComponent<PlayerHealth>();
            PlayerInput = other.gameObject.GetComponent<PlayerInput>();

            //Shake Camera
            CameraShaker.Instance.ShakeOnce(3f, 3f, .1f, 1f);

            PlayerInput.KnockbackCounter = PlayerInput.KnockbackTime;
            if(other.transform.position.x <= transform.position.x)
            {
                PlayerInput.KnockFromRight = true;
            }
            if (other.transform.position.x > transform.position.x)
            {
                PlayerInput.KnockFromRight = false;
            }
            playerHealth.takeDamage(damage);
            PlayerAudio.instance.PlaySound("Knife Damage");
        }
    }
}
