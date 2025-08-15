using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [HideInInspector]
    public float lastHorizontalVector;
    [HideInInspector]
    public float lastVerticalVector;
    [HideInInspector]
    public Vector2 moveDir;
    [HideInInspector]
    public Vector2 lastMoveVector;

    Rigidbody2D rb;
    PlayerStats player;

    void Start()
    {
        player = GetComponent<PlayerStats>();
        rb = GetComponent<Rigidbody2D>();
        lastMoveVector = new Vector2(1, 0f); 
    }

    void Update()
    {
        inputManagement();
    }

    void FixedUpdate()
    {
        move();
    }

    void inputManagement()
    {
        if(GameManager.instance.isGameOver)
        {
            return;
        }
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveDir = new Vector2(moveX, moveY).normalized;

        if(moveDir.x != 0)
        {
            lastHorizontalVector = moveDir.x;
            lastMoveVector = new Vector2(lastHorizontalVector, 0f); // last moved x
        }
        if(moveDir.y != 0)
        {
            lastVerticalVector = moveDir.y;
            lastMoveVector = new Vector2(0, lastVerticalVector); // last moved y
        }

        if(moveDir.x != 0 && moveDir.y != 0) 
        {
            lastMoveVector = new Vector2(lastHorizontalVector, lastVerticalVector); //while moving
        }
    }

    void move()
    {
        if (GameManager.instance.isGameOver)
        {
            return;
        }
        rb.velocity = new Vector2 (moveDir.x * player.CurrentMoveSpeed, moveDir.y * player.CurrentMoveSpeed);
    }
}
