using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;
using System.Data.Common;
using System.Linq;
using TMPro;

public class Player : MonoBehaviour
{
    public float magnitude, roughness, fadein, fadeout;
    public GameObject parent;
    public bool invincible;

    Healthbar healthbar;
    public TMP_Text coinsText;
    public ParticleSystem deathParticles;
    bool deathParticlesPlayed;

    private SpriteRenderer spriteRenderer;

    //public static List<int> grades = new List<int>();
    //public static int coins;

    public float maxHealth = 10.00f;
    public float minHealth = 5.00f;

    private void Start()
    { 
        spriteRenderer = GetComponent<SpriteRenderer>();
        healthbar = FindObjectOfType<Healthbar>();
        parent = transform.parent.gameObject;
        //currentHealth = maxHealth; // this is a problem. Need to implement singleton to avoid overriding health value on character switch
        //grades.Add(10);
    }

    private void Update()
    { 
        if (Input.GetKeyDown("g"))
        {
            TakeDamage(9);
        }

        coinsText.text = string.Concat("x" + GameMaster.Instance.coins.ToString());
        deathParticles.transform.position = transform.position;
        healthbar.SetHealth(GameMaster.Instance.currentHealth);
        if (GameMaster.Instance.currentHealth < minHealth)
        {
            Die();
        }

    }

    public void TakeDamage(int damage)
    {
        Shield shield = FindObjectOfType<Shield>();
        if (GameMaster.Instance.currentHealth >= 5f)
        {
            if (!shield && !invincible)
            {
                GameMaster.Instance.currentHealth = (damage + GameMaster.Instance.currentHealth) / 2;
                Debug.Log("Current health: " + GameMaster.Instance.currentHealth);
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
                //grades.Add(damage);
                GameMaster.Instance.grades.Add(damage);
                //Debug.Log("Total grades: " + grades.Count);
            } else
            {
                shield.TakeDamage(damage);
            }
        }
    }

    public void Die()
    {
        if (!deathParticlesPlayed)
        {
            deathParticles.Play();
            transform.parent.gameObject.SetActive(false);
            deathParticlesPlayed = true;
        }
        FindObjectOfType<AudioManager>().GetComponents<AudioSource>()[13].enabled = false;
        FindObjectOfType<characterSwitch>().dead = true;
        FindObjectOfType<GameManager>().GameOver();
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
