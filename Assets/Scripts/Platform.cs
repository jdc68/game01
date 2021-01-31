using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public Transform pos1, pos2;
    public float speed;
    public Transform startPos;
    private Transform targetParent;
    bool playerOnPlatform;
    Vector3 nextPos;
    BoxCollider2D collider2D;

    // Start is called before the first frame update
    void Start()
    {
        nextPos = startPos.position;
        collider2D = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position == pos1.position)
            nextPos = pos2.position;
        else if (transform.position == pos2.position)
            nextPos = pos1.position;

        transform.position = Vector3.MoveTowards(transform.position, nextPos, speed * Time.deltaTime);

        //if (playerOnPlatform && Input.GetKeyDown(KeyCode.S))
        //.transform.parent = targetParent;
        //    targetParent = null;
        //    playerOnPlatform = false;
        //    collider2D.enabled = false;
        //    Invoke("EnableCollider", .5f);
        //}
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(pos1.position, pos2.position);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            targetParent = collision.collider.transform.parent;
            collision.collider.transform.SetParent(transform);
            playerOnPlatform = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.collider.transform.SetParent(targetParent);
            targetParent = null;
            playerOnPlatform = false;
        }
    }
}
