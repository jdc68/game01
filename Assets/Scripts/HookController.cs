using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class HookController : MonoBehaviour
{
    private Rigidbody2D rb;
    [HideInInspector]
    public bool exists;
    public bool collided;
    private Transform targetParent;
    [HideInInspector]
    public Transform firePoint;
    public float hookSpeed;
    private float facingInitially;
    public float destroyRadius;
    public LayerMask layerMask;
    public SpriteRenderer chain;
    public float chainMaxLength;
    public float hookDamage;
    private bool enemyHooked;
    private Transform enemy;

    void Start()
    {
        firePoint = transform.parent.Find("Arm").GetChild(0).transform;
        facingInitially = transform.eulerAngles.y;

        rb = GetComponent<Rigidbody2D>();
        exists = true;

        rb.velocity = firePoint.right * hookSpeed;
    }

    private void Update()
    {
        firePoint = transform.parent.Find("Arm").GetChild(0).transform;

        Vector2 direction = firePoint.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        chain.size = new Vector2(-Vector2.Distance(transform.position, firePoint.position), chain.size.y);

        if (transform.childCount > 1)
            transform.GetChild(1).GetComponent<Rigidbody2D>().position = rb.position;

        if (facingInitially < 180)
            rb.rotation = angle - 180;
        else
            rb.rotation = angle;

        if (collided)
            rb.velocity = direction.normalized * hookSpeed;

        if (Vector2.Distance(transform.position, firePoint.position) < 0.5f && collided)
        {
            Destroy();
        }

        if (Vector2.Distance(transform.position, firePoint.position) >= chainMaxLength && !collided)
        {
            collided = true;
        }

        Collider2D other = Physics2D.OverlapCircle(transform.position, destroyRadius, layerMask);
        if (other != null && collided)
        {
            
            Destroy();
        }

        if (enemyHooked && enemy)
        {
            enemy.transform.position = transform.position;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision != null)
        {
            if (collision.transform.tag != "Player")
            {
                rb.velocity = Vector2.zero;
                collided = true;
            }

            if (collision.transform.tag == "enemy" && transform.childCount <= 1)
            {
                enemyHooked = true;
                enemy = collision.transform;
                enemy.transform.position = transform.position;
                collision.transform.GetComponent<EnemyHP>().TakeDamage(hookDamage);
                DamageIndicator.Create(collision.transform.position, hookDamage);
            }
            Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), collision.collider);

        }
    }

    public void Destroy()
    {
        if (enemyHooked)
        {
            Debug.Log("test");
            LaunchHook controller = transform.parent.Find("Arm").GetComponent<LaunchHook>();
            controller.onCooldown = true;
            controller.StartCooldown();
        }
        enemyHooked = false;
        exists = false;
        transform.parent.Find("Arm").GetComponent<LaunchHook>().hooked = false;
        if (targetParent != null)
        {
            transform.GetChild(1).SetParent(targetParent);
        }
        targetParent = null;
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, destroyRadius);
    }
}
