using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class ArmMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public Camera cam;
    Vector2 mousePosition;
    public GameObject player;
    public float xOffset = 0;
    public float yOffset = 0;

    private bool facingRight = true;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 lookDir = mousePosition - rb.position;
        if(mousePosition.x > rb.position.x )
        {
            float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
            rb.rotation = angle;
            if (!facingRight)
                Flip();
            
        }
        else if (mousePosition.x < rb.position.x)
        {
            float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 180f;
            rb.rotation = angle;
            if (facingRight)
                Flip();
        }
    }

    private void LateUpdate()
    {
        if (facingRight)
        {
            transform.position = player.transform.position + new Vector3(xOffset, yOffset, 0);
        } else
        {
            transform.position = player.transform.position + new Vector3(-xOffset, yOffset, 0);

        }

    }
    void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
        player.transform.Rotate(0, 180, 0);
    }
}
