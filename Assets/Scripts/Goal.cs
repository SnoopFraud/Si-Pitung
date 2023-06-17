using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField] private GameObject GoalUI;

    private bool isPaused;

    private void Start()
    {
        GoalUI.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player Reach the Goal!");
            GoalUI.SetActive(true);

            Time.timeScale = 0f;
        }
    }
}
