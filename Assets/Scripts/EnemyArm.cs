using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArm : MonoBehaviour
{
    public GameObject body;
    public Rigidbody2D rb;
    private bool facingRight = false;
    public float yOffset;

    public Transform nextPos;
    public Rigidbody2D player;
    public bool playerNearby;

    void Update()
    {
        nextPos = body.GetComponent<Enemy>().nextPos;
        player = body.GetComponent<Enemy>().player;
        playerNearby = body.GetComponent<Enemy>().playerNearby ;

        transform.position = body.transform.position + new Vector3(0f, yOffset, 0f);
        if (playerNearby)
        {
            Vector2 lookDir = player.position - rb.position;
            if (player.position.x > rb.position.x)
            {
                float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
                rb.rotation = angle;
                if (!facingRight)
                    Flip();

            }
            else if (player.position.x < rb.position.x)
            {
                float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 180f;
                rb.rotation = angle;
                if (facingRight)
                    Flip();
            }
        } else if (nextPos)
        {
            rb.rotation = 0;
            if (nextPos.position.x > transform.position.x)
            {
                if (!facingRight)
                    Flip();

            }
            else if (nextPos.position.x < transform.position.x)
            {
                if (facingRight)
                    Flip();
            }
        }
        
    }

    void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }
}
