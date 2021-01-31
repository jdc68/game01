using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    public float maxHealth, currentHealth;
    public Healthbar_enemy healthbar;
    public ParticleSystem particles;
    public GameObject loot;
    [Range(0, 100)]
    public float chanseToDrop;
    public float damageRate;
    public float repeatingDamageAmount;
    public bool takingDamage;


    private Transform parent;
    public SpriteRenderer spriteRenderer;

    private void Start()
    {
        currentHealth = maxHealth;
        healthbar.SetMaxHealth(maxHealth);
        if (!spriteRenderer)
            spriteRenderer = GetComponent<SpriteRenderer>();
        parent = transform.parent;
        InvokeRepeating("TakeDamage_", 0f, .5f);
    }

    public void TakeDamage(float damage)
    {
        FindObjectOfType<AudioManager>().Play("Hit");
        StartCoroutine(hurtGFX());
        currentHealth -= damage;
        healthbar.SetHealth(currentHealth);
        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        Instantiate(particles, transform.position, Quaternion.identity);
        var a = Random.Range(0, 100);
        if (chanseToDrop > a)
        {
            Instantiate(loot, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
        Destroy(parent.gameObject);
    }
    IEnumerator hurtGFX()
    {
        spriteRenderer.color = new Color(1, .34f, .34f, 1);
        try
        {
            parent.Find("arm").GetComponent<SpriteRenderer>().color = new Color(1f, 0.34f, 0.34f);
        }
        catch {}
        yield return new WaitForSeconds(.25f);
        spriteRenderer.color = Color.white;
        try
        {
            parent.Find("arm").GetComponent<SpriteRenderer>().color = Color.white;
        }
        catch {}
    }

    public void TakeDamage_()
    {
        if (takingDamage)
        {
            TakeDamage(repeatingDamageAmount);
        }
    }
}
