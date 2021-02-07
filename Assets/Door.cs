using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public float playerDetectionRadius;
    public bool open;
    Transform player;
    public Transform doorMain;
    public Canvas doorUI;


    private void Start()
    {
        player = FindObjectOfType<PlayerMovementController>().transform;
    }

    private void Update()

    {
        player = FindObjectOfType<PlayerMovementController>().transform;

        Vector2 distanceToPlayer = player.position - transform.position;
        if (distanceToPlayer.magnitude < playerDetectionRadius)
        {
            doorUI.gameObject.SetActive(true);
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (!open)
                {
                    doorMain.rotation = new Quaternion(0, 180, 0, 0);
                    open = true;
                } else
                {
                    doorMain.rotation = Quaternion.identity;
                    open = false;
                }
                
            }
        } else
        {
            doorUI.gameObject.SetActive(false);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, playerDetectionRadius);
    }
}
