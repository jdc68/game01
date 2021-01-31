using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicator : MonoBehaviour
{
    Camera cam;
    public LayerMask cameraLayer;
    public GameObject indicator;
    public bool showIndicator;
    float minDistanceToCamera;
    [Range(0, 1)]
    public float indicatorOpacityStrength = 1;
    bool showGizmos;

    void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        minDistanceToCamera = FindObjectOfType<CameraCollision>().enemyDetectionRadius;
        var distance = Vector2.Distance(transform.position, cam.transform.position);
        if (distance < minDistanceToCamera)
        {
            if (showIndicator)
            {
                //Make indicator active
                if (!indicator.activeSelf)
                {
                    indicator.SetActive(true);
                }
                Vector2 direction = cam.transform.position - transform.position;
                RaycastHit2D ray = Physics2D.Raycast(transform.position, direction, Mathf.Infinity, cameraLayer);
                if (ray.collider != null)
                {
                    //Position indicator
                    indicator.transform.position = ray.point;

                    //Rotate indicator
                    Quaternion rotation = Quaternion.LookRotation(direction, transform.TransformDirection(Vector3.up));
                    indicator.transform.rotation = new Quaternion(0, 0, rotation.z, rotation.w);
                    if (indicator.transform.position.x < cam.transform.position.x)
                    {
                        indicator.GetComponent<SpriteRenderer>().flipX = true;
                    } else
                    {
                        indicator.GetComponent<SpriteRenderer>().flipX = false;

                    }
                }

                // Resize indicator
                var scale = Mathf.Clamp((minDistanceToCamera / distance), 1, 2);
                indicator.transform.localScale = new Vector3(scale, scale, scale);

                // Indicator opacity
                indicator.GetComponent<Renderer>().sortingOrder = Mathf.RoundToInt(scale);
                float indicatorAlpha = scale - indicatorOpacityStrength;
                Color indicatorColor = indicator.GetComponent<SpriteRenderer>().color;
                Color indicatorColor_ = new Color(indicatorColor.r, indicatorColor.g, indicatorColor.b, indicatorAlpha);
                indicator.GetComponent<SpriteRenderer>().color = indicatorColor_;
            }
        }
        else
        {
            indicator.SetActive(false);
        }
        if (!showIndicator)
        {
            showGizmos = false;
            indicator.SetActive(false);
        }
    }

    //private void OnDrawGizmos()
    //{
    //    if (showGizmos)
    //    {
    //        Vector2 direction = cam.transform.position - transform.position;
    //        RaycastHit2D ray = Physics2D.Raycast(transform.position, direction, Mathf.Infinity, cameraLayer);
    //        if (ray.collider != null)
    //        {
    //            Debug.Log(ray.collider.name);
    //            Gizmos.DrawWireSphere(ray.point, 0.5f);
    //            Gizmos.DrawLine(transform.position, ray.point);
    //        }
    //    }
    //}
}
