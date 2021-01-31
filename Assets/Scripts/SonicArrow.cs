using Microsoft.Win32;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonicArrow : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool hasHit;
    public float damage;
    public int maxHits;
    public int hits = 0;
    private Vector2 lastVelocity;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity *= 1.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasHit)
        {
            float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            lastVelocity = rb.velocity;
            if (rb.velocity.magnitude < 35)
            {
                rb.velocity += new Vector2(1f, 1f);
            } 
        }

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer != 13 && collision.gameObject.layer != 17 && collision.gameObject.layer != 11)
        {
            if (collision.transform.tag == "enemy")
            {
                //Enemy enemy = collision.transform.GetComponent<Enemy>();
                //if (enemy != null)
                //{
                //    enemy.TakeDamage(damage);
                //    DamageIndicator.Create(enemy.transform.position, damage);
                //    FindObjectOfType<AudioManager>().Play("ArrowIn");
                //    hits = maxHits;
                //}
            }
            if (hits < maxHits)
            {
                hits++;
                rb.velocity = Vector3.Reflect(lastVelocity, collision.contacts[0].normal);
            } else
            {
                transform.parent = collision.transform;
                hasHit = true;
                rb.velocity = Vector2.zero;
                rb.isKinematic = true;
                gameObject.layer = 14;
                GetComponent<TrailRenderer>().emitting = false;
                GetComponent<CircleCollider2D>().enabled = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 15 && 
            collision.gameObject.layer != 13 && 
            collision.gameObject.layer != 17 && 
            collision.gameObject.layer != 11)
        {
            EnemyHP enemy = collision.transform.GetComponent<EnemyHP>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                DamageIndicator.Create(enemy.transform.position, damage);
                FindObjectOfType<AudioManager>().Play("ArrowIn");
                hits = maxHits;
            }
            transform.parent = collision.transform;
            hasHit = true;
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
            gameObject.layer = 14;
            GetComponent<TrailRenderer>().emitting = false;
            GetComponent<CircleCollider2D>().enabled = false;
        }
    }
}
