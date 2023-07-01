using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeluruTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Wall"))
        {
            PlayerAudio.instance.PlaySound("Peluru DMG");
            gameObject.SetActive(false);
        }
    }
}
