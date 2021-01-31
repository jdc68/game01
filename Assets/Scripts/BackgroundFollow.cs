using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundFollow : MonoBehaviour
{
    public GameObject backgroundParent;
    public GameObject background;
    private Transform cam;
    private Vector3 lastCameraPosition;
    public List<GameObject> backgroundInstance = new List<GameObject>();
    public float parallaxMultiplier = .5f;
    public bool moveOnYAxis;

    float width;

    void Start()
    {
        cam = transform.parent.transform;
        width = background.GetComponent<SpriteRenderer>().bounds.size.x;
        lastCameraPosition = cam.position;
    }

    void Update()
    {
        if (backgroundInstance.Count > 0)
        {
            for (var i = 0; i < backgroundInstance.Count; i++)
            {
                if (cam.position.x > backgroundInstance[i].transform.position.x &&
                    cam.position.x <= backgroundInstance[i].transform.position.x + width)
                {
                    Vector3 spawnAt = backgroundInstance[i].transform.position + new Vector3(width, 0, 0);
                    if (CheckSpawn(spawnAt) && backgroundInstance.Count < 9)
                        backgroundInstance.Add(Instantiate(background, spawnAt, cam.rotation, backgroundParent.transform));
                }          
            }

            for (var i = 0; i < backgroundInstance.Count; i++)
            {
                if (cam.position.x < backgroundInstance[i].transform.position.x &&
                    cam.transform.position.x >= backgroundInstance[i].transform.position.x - width)
                {
                    Vector3 spawnAt = backgroundInstance[i].transform.position - new Vector3(width, 0, 0);
                    if (CheckSpawn(spawnAt) && backgroundInstance.Count < 9)
                        backgroundInstance.Add(Instantiate(background, spawnAt, cam.rotation, backgroundParent.transform));
                }    
            }
        }
    }

    private void LateUpdate()
    {
        Vector3 deltaMovement = cam.position - lastCameraPosition;
        if (moveOnYAxis)
        {
            backgroundParent.transform.position += deltaMovement * parallaxMultiplier;
        }
        else
        {
            Vector3 newPosition = new Vector3(backgroundParent.transform.position.x + deltaMovement.x * parallaxMultiplier, backgroundParent.transform.position.y, backgroundParent.transform.position.z);
            backgroundParent.transform.position = newPosition;
        }
        lastCameraPosition = cam.position;

        for (var i = 0; i < backgroundInstance.Count; i++)
        {
            if (cam.position.x < backgroundInstance[i].transform.position.x - width * 2 |
                cam.position.x > backgroundInstance[i].transform.position.x + width * 2)
            {
                Debug.Log("test111");
                Destroy(backgroundInstance[i]);
                backgroundInstance.RemoveAt(i);
            }
        }
    }

    bool CheckSpawn(Vector3 pos)
    {
        bool allowSpawn = true;
        for (var i = 0; i < backgroundInstance.Count; i++)
        {
            if (backgroundInstance[i].transform.position == pos)
            {
                allowSpawn = false;
                break;
            }
        }
        return allowSpawn;

    }
}
