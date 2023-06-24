using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform targetplayer;
    public Vector3 offset;
    public float smoothspeed = 0.125f;
    public float smoothtime = 0.3f;
    private Vector3 velocity = Vector3.zero;

    private void Start()
    {
        targetplayer = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void FixedUpdate()
    {
        Vector3 DesiredPosition = targetplayer.position + offset;
        Vector3 SmoothPosition = Vector3.SmoothDamp(
            transform.position, DesiredPosition, ref velocity, smoothtime);
        transform.position = SmoothPosition;
    }

    //References
    /*
     * Camera Follow: https://youtu.be/MFQhpwc6cKE
     */
}
