using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleTargetCamera : MonoBehaviour
{
    Camera_Control cameraControl;
    Collider2D[] targets;

    private void Start()
    {
        cameraControl = GetComponent<Camera_Control>();
        targets = cameraControl.targets;
    }

    private void LateUpdate()
    {
        
    }

    
}
