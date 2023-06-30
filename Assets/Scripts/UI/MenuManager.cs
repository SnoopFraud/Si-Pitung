using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public void Quitting()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }
}
