using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class checkPlayerNearby : MonoBehaviour
{
    public LayerMask playerLayer;
    public Animator animator;

    public void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (Physics2D.OverlapCircle(transform.position, 8f, playerLayer))
        {
            animator.SetBool("open", true);
        } else
        {
            animator.SetBool("open", false);
        }
    }
}
