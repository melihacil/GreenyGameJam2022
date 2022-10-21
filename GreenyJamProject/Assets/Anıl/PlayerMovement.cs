using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed = 5f;
    public Rigidbody2D rb;
    public Vector2 playerDirection;
    

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }



    void Update()
    {
        float directionX = Input.GetAxisRaw("Horizontal");
        float directionY = Input.GetAxisRaw("Vertical");
        playerDirection = new Vector2(directionX, directionY).normalized;
        
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(playerDirection.x*movementSpeed,playerDirection.y * movementSpeed);
        
    }
}
