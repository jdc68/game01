using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollision : MonoBehaviour
{
    Camera Cam;
    BoxCollider2D Box;

    public float enemyDetectionRadius;
    public LayerMask enemyLayer;
    Collider2D[] enemiesClose;
    public float padding;


    // Start is called before the first frame update
    void Start()
    {
        Cam = Camera.main;
        Box = GetComponent<BoxCollider2D>();

        if (!Cam.orthographic)
        {
            Debug.LogError("Camera must be Orthographic.");
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        var aspect = (float)Screen.width / Screen.height;
        var orthoSize = Cam.orthographicSize;

        var width = 2.0f * orthoSize * aspect;
        var height = 2.0f * Cam.orthographicSize;

        Box.size = new Vector2(width - padding, height - padding);

        enemiesClose = Physics2D.OverlapCircleAll(Cam.transform.position, enemyDetectionRadius, enemyLayer);
        foreach(Collider2D enemy in enemiesClose)
        {
            if (!CanSee(enemy.gameObject))
            {
                enemy.GetComponent<Indicator>().showIndicator = true;
            } else
            {
                enemy.GetComponent<Indicator>().showIndicator = false;
            }
        }
    }

    private bool CanSee(GameObject gameObject)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Cam);
        if (GeometryUtility.TestPlanesAABB(planes, gameObject.GetComponent<BoxCollider2D>().bounds))
            return true;
        else
            return false;
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawWireSphere(Cam.transform.position, enemyDetectionRadius);
    //    foreach (Collider2D enemy in enemiesClose)
    //    {
    //        if (!CanSee(enemy.gameObject))
    //        {
    //            Gizmos.DrawLine(Cam.transform.position, enemy.transform.position);
    //        }
    //    }
    //}
}
