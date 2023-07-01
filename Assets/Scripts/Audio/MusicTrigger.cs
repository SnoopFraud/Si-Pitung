using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTrigger : MonoBehaviour
{
    public string MusicName;

    private void Start()
    {
        MusicManager.instance.PlaySound(MusicName);
    }
}
