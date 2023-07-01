using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemiesLeft : MonoBehaviour
{
    public TextMeshProUGUI enemytxt;
    [SerializeField] private int EnemyLeft;

    private void Start()
    {
        
    }

    private void Update()
    {
        EnemyUpdate();
    }

    private void EnemyUpdate()
    {
        EnemyLeft = GameManager.instance.CountEnemies;
        enemytxt.text = "Tersisa " + EnemyLeft + " Musuh";
    }
}
