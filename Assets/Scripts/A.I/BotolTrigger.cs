using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotolTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Wall"))
        {
            PlayerAudio.instance.PlaySound("Botol DMG");
            gameObject.SetActive(false);
        }
    }
}
