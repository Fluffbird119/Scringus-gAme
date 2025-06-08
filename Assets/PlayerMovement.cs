using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    Vector2 moveInput;
    public int moveSpeed = 5;

    void Start()
    {
        
    }

    void Update()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal"); // only -1 or 0 or 1 if you use GetAxis instead it will smoothly transfer from -1 to 0 to 1 if you would want that for whatever reason (perhaps a fish character that slides around)
        moveInput.y = Input.GetAxisRaw("Vertical");

        //moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")); // can also be written as this

        moveInput.Normalize(); // so diagonal movement is just as fast as up and down movement
    }

    private void FixedUpdate()
    {
        rb.velocity = moveInput * moveSpeed;
    }
}
