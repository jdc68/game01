using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableButton : MonoBehaviour
{
    public void Disable()
    {
        gameObject.SetActive(false);
    }
}
