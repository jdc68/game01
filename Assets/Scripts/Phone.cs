using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Phone : MonoBehaviour
{
    public Transform pos1, pos2;
    public float phoneSpeed = 1f;
    //public bool phoneUp = false;
    Animator animator;
    //public bool calling = false;
    public Sprite[] sprites;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse2) && !ScoreManager.Instance.isDead)
        {
            if (PhoneManager.Instance.phoneUp)
                PhoneManager.Instance.phoneUp = false;
            else
                PhoneManager.Instance.phoneUp = true;
        }

        if (PhoneManager.Instance.phoneUp)
            animator.SetBool("phoneUp", true);
        else
            animator.SetBool("phoneUp", false);

        if (PhoneManager.Instance.calling )
        {
            PhoneManager.Instance.phoneUp = true;
            if (!AudioManager.Instance.GetComponents<AudioSource>()[32].isPlaying)
                AudioManager.Instance.Play("phoneCall");
            GetComponent<Image>().sprite = sprites[1];
        } else
        {
            AudioManager.Instance.Stop("phoneCall");
            GetComponent<Image>().sprite = sprites[0];
        }
    }
}
