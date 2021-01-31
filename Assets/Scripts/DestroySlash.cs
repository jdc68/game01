using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySlash : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(waitDestroy());   
    }
    IEnumerator waitDestroy()
    {
        yield return new WaitForSeconds(.2f);
        Destroy(gameObject);
    }
}

