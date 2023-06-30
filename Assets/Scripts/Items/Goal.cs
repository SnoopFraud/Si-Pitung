using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public string NextLevel;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && GameManager.instance.isEnd)
        {
            GameManager.instance.NextLv(NextLevel);
            GameManager.instance.Win();
        }
    }
}
