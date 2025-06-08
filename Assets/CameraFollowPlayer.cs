using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    public Transform player;
    public float smoothTime = 0.3F;
    Vector3 velocity = Vector3.zero;
    private float visionSize = 5f;

    void Start()
    {
        transform.position = player.position + new Vector3(0, 0, -visionSize);
    }

    void Update()
    {
        
        Vector3 targetPosition = player.TransformPoint(new Vector3(0, 0, -visionSize));

        // Smoothly move the camera towards that target position
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        

        //transform.position = player.transform.position + new Vector3(0, 0, -1.5f);
    }
}
