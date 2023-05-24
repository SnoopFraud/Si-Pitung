using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLevel : MonoBehaviour
{
    #region var
    GameObject Doortext;
    #endregion

    private void Start()
    {
        Doortext = GameObject.Find("Canvas/Door Text");
        Doortext.SetActive(false);
    }

    private void OnTriggerEnter(Collider thisobject)
    {
        if(thisobject.gameObject.tag == "Player")
        {
            Doortext.SetActive(true);
            Debug.Log(thisobject.gameObject.name + " Detected");
        }
    }

    private void OnTriggerExit(Collider thisobject)
    {
        Doortext.SetActive(false);
    }
}
