using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;
using System.Data.Common;
using System.Linq;

public class Player : MonoBehaviour
{
    public float magnitude, roughness, fadein, fadeout;
    public GameObject parent;
    public List<int> grades = new List<int>();
    public bool invincible;

    public Health mutualGrades;
    private SpriteRenderer spriteRenderer;

    private void Start()
    { 
        spriteRenderer = GetComponent<SpriteRenderer>();

        parent = transform.parent.gameObject;
    }

    private void Update()
    { 
        if (Input.GetKeyDown("g"))
        {
            TakeDamage(1);
        }
    }

    public void TakeDamage(int damage)
    {
        Shield shield = FindObjectOfType<Shield>();
        if (mutualGrades.currentHealth >= 5f)
        {
            if (!shield && !invincible)
            {
                mutualGrades.currentHealth = (damage + mutualGrades.currentHealth) / 2;
                if (damage < 9)
                {
                    FindObjectOfType<AudioManager>().Play("Hurt");

                    try
                    {
                        CameraShaker.Instance.ShakeOnce(magnitude, roughness, fadein, fadeout);
                    }
                    catch { }
                    StartCoroutine(hurtGFX());
                }
                grades.Add(damage);
                mutualGrades.grades.Add(damage);
            } else
            {
                shield.TakeDamage(damage);
            }
        }
    }

    IEnumerator hurtGFX()
    {
        spriteRenderer.color = new Color(1, .34f, .34f, 1);
        try
        {
            transform.parent.Find("Arm").GetComponent<SpriteRenderer>().color = new Color(1f, 0.34f, 0.34f);
        } catch { }
        yield return new WaitForSeconds(.25f);
        spriteRenderer.color = Color.white;
        try
        {
            transform.parent.Find("Arm").GetComponent<SpriteRenderer>().color = Color.white;
        } catch { }
    }
}
