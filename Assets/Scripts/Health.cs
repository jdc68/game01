using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Health : MonoBehaviour
{
    public List<int> grades = new List<int>();

    public int coins;
    public float maxHealth = 10.00f;
    public float minHealth = 5.00f;
    public float currentHealth;

    public Healthbar healthbar;
    public TMP_Text coinsText;
    public ParticleSystem deathParticles;
    public GameObject activeCharacter;
    private bool deathParticlesPlayed;

    private void Awake()
    {
        coins = FindObjectOfType<GameManager>().coins;
        deathParticlesPlayed = false;
    }
    private void Start()
    {
        currentHealth = maxHealth;
        healthbar.SetMaxHealth(maxHealth);
        activeCharacter = transform.GetComponent<characterSwitch>().activeCharacter;
    }

    // Update is called once per frame
    void Update()
    {
        FindObjectOfType<GameManager>().coins = coins;
        activeCharacter = transform.GetComponent<characterSwitch>().activeCharacter;
        deathParticles.transform.position = activeCharacter.transform.position;
        coinsText.text = string.Concat("x" + coins.ToString());

        //for (int i = 0; i < grades.Count; i++)
        //{
        //    currentHealth = (currentHealth + grades[i]) / 2;
        //}

        healthbar.SetHealth(currentHealth);
    }

    void FixedUpdate()
    {
        if (currentHealth < minHealth)
        {
            Die();
        }
    }

    public void Die()
    {
        if (!deathParticlesPlayed)
        {
            deathParticles.Play();
            activeCharacter.transform.parent.gameObject.SetActive(false);
            deathParticlesPlayed = true;
        }
        FindObjectOfType<AudioManager>().GetComponents<AudioSource>()[13].enabled = false;
        GetComponent<characterSwitch>().dead = true;
        FindObjectOfType<GameManager>().GameOver();
    }
}
