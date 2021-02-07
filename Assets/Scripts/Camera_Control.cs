using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Control : MonoBehaviour
{
    public float smoothSpeed = 0.125f;
    public float yOffset = 0;
    public Collider2D[] targets;
    public Vector3 overlapBoxSize;
    public LayerMask mask;
    public bool adaptiveCamera = true;
    Vector3 targetPosition;
    private Vector3 velocity;

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

    private void FixedUpdate()
    {
        try
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        catch { }
        if (player)
        {
            if (adaptiveCamera)
            {
                targets = Physics2D.OverlapBoxAll(player.position, overlapBoxSize, 0, mask);
                targetPosition = GetCentrePoint() + new Vector3(0, yOffset, 0);
            } else
            {
                targetPosition = player.position + new Vector3(0, yOffset, 0);
            }

            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothSpeed);   
        }
        
    }

    Vector3 GetCentrePoint()
    {
        if (targets.Length == 0)
            return player.position;

        if (targets.Length == 1)
        {
            return targets[0].transform.position;
        }

        var bounds = new Bounds(targets[0].transform.position, Vector3.zero);
        for (int i = 0; i < targets.Length; i++)
        {
            bounds.Encapsulate(targets[i].transform.position);
        }

        return bounds.center;
    }

    private void OnDrawGizmos()
    {
        if (player)
            Gizmos.DrawWireCube(player.position, overlapBoxSize);
    }
}
