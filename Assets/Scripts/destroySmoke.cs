using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroySmoke : MonoBehaviour
{
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(DestroyTimer());
    }

    IEnumerator DestroyTimer()
    {
        animator.Play("smoke_default");
        yield return new WaitForSeconds(0.8f);
        Destroy(gameObject);
    }
}
