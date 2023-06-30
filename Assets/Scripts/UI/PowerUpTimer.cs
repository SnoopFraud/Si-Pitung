using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PowerUpTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timertxt;
    private float starttime = 0f;

    private void Awake()
    {
        gameObject.SetActive(false);
    }
}
