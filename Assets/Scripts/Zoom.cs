using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zoom : MonoBehaviour
{
    public float scrollSpeed;
    public float zoomSpeed;
    public float minZoom, maxZoom;
    private Camera cam;
    private float distance;

    private void Start()
    {
        cam = Camera.main;
        distance = cam.orthographicSize;
    }
    // Update is called once per frame
    void Update()
    {
        if (!ScoreManager.Instance.isDead)
        {
            distance -= Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
            distance = Mathf.Clamp(distance, minZoom, maxZoom);
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, distance, zoomSpeed);
            //Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
        }
    }
}
