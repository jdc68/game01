using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Phone : MonoBehaviour
{
    public Transform pos1, pos2;
    public float phoneSpeed = 1f;
    public bool phoneUp = false;
    Animator animator;
    public bool calling = false;
    public Sprite[] sprites;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse2) && !FindObjectOfType<GameManager>().dead)
        {
            if (phoneUp)
            {
                phoneUp = false;
                animator.SetBool("phoneUp", false);
            }
            else
            {
                phoneUp = true;
                animator.SetBool("phoneUp", true);
            }
        }

        if (calling)
        {
            if (!FindObjectOfType<AudioManager>().GetComponents<AudioSource>()[32].isPlaying)
                FindObjectOfType<AudioManager>().Play("phoneCall");
            GetComponent<Image>().sprite = sprites[1];
        } else
        {
            FindObjectOfType<AudioManager>().Stop("phoneCall");
            GetComponent<Image>().sprite = sprites[0];
        }
    }
}
