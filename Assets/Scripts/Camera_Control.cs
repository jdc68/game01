using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Control : MonoBehaviour
{
    public float smoothSpeed = 0.125f;
    public float yOffset = 0;

    private Transform player;
    private void Start()
    {
        try
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        catch { }
        transform.position = player.position;
    }
    private void Update()
    {
        try {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        catch { }
    }

    void FixedUpdate()
    {
        if (player)
        {
            Vector3 targetPosition = player.position + new Vector3(0, yOffset, 0);
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }
}
