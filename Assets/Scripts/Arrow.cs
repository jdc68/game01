using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool hasHit;
    public float damage;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasHit)
        {
            float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != 13 
            && collision.gameObject.layer != 14 
            && collision.gameObject.layer != 17
            && collision.gameObject.layer != 21)
        {
            GetComponent<CircleCollider2D>().enabled = false;
            rb.velocity = Vector2.zero;
            transform.parent = collision.transform;
            hasHit = true;
            rb.isKinematic = true;
            gameObject.layer = 14;
            GetComponent<TrailRenderer>().emitting = false;
        }
        
        EnemyHP enemy = collision.GetComponent<EnemyHP>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            DamageIndicator.Create(enemy.transform.position, damage);
            FindObjectOfType<AudioManager>().Play("ArrowIn");
        }
    }
}
