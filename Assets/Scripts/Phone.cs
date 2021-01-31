using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Phone : MonoBehaviour
{
    public Transform pos1, pos2;
    public float phoneSpeed = 1f;
    public bool phoneUp = false;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = pos2.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse2) && !FindObjectOfType<GameManager>().dead)
        {
            if (phoneUp)
                phoneUp = false;
            else
                phoneUp = true;
        }
    }

    void FixedUpdate()
    {
        if (phoneUp)
        {
            Vector2 dir = pos1.position - transform.position;
            if (transform.position.y <= pos1.position.y)
            {
                transform.Translate(dir.normalized * phoneSpeed, Space.World);
            }
        }
        else
        {
            Vector2 dir = pos2.position - transform.position;
            if (transform.position.y >= pos2.position.y)
            {
                transform.Translate(dir.normalized * phoneSpeed, Space.World);
            }
        }

    }
}
