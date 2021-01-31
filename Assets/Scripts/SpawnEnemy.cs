using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class SpawnEnemy : MonoBehaviour
{
    public GameObject enemy;
    public bool continuous;
    public List<GameObject> spawnedObjects = new List<GameObject>();
    private float timeToSpawn;
    public float spawnRate;

    // Update is called once per frame
    void Update()
    {
        if (continuous)
        {
            if (Input.GetKey(KeyCode.Mouse1) && Time.time > timeToSpawn)
            {
                Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                GameObject instance = Instantiate(enemy, pos, Quaternion.identity, transform);
                spawnedObjects.Add(instance);
                timeToSpawn = Time.time + spawnRate;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Instantiate(enemy, pos, Quaternion.identity, transform);
            }
        }

        if (Input.GetKeyDown("v"))
        {
            foreach (GameObject item in spawnedObjects)
            {
                if (item != null)
                    Destroy(item);
            }
        }
    }
}
