using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Roll : MonoBehaviour
{
    public Transform circleToRotate; // assign your circle child here
    private float rollSpeed = 90f; // degrees per unit moved
    public Animator animator;

    private Vector3 lastPosition;

    void Start()
    {
        lastPosition = transform.position;
    }

    void Update()
    {
        Vector3 delta = transform.position - lastPosition;
        float xMoved = MathF.Abs(delta.x);
        /*
        if (lastPosition.y != transform.position.y)
        {
            animator.SetBool("isMoving", true);
        } 
        else
        {
            animator.SetBool("isMoving", false);
        }*/

            // Rotate in the opposite direction to movement
            float xRotation = -xMoved * rollSpeed;
        circleToRotate.Rotate(0, 0, xRotation);

        lastPosition = transform.position;
    }
}
