using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 5;
    public float bulletSpeed;
    protected private Rigidbody2D rb;
    public float distanceToFly = 100f;
    public GameObject destroyEffect;
    private float timeToDestroy = 1f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {     
        if (Mathf.Abs(transform.position.x) > distanceToFly)
        {
            Destroy(gameObject);
        }       
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name != "Player")
        {
            EnemyHP enemy = other.GetComponent<EnemyHP>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                DamageIndicator.Create(enemy.transform.position, damage);
            }
            Instantiate(destroyEffect, transform.position, transform.rotation);
            if (Time.time > timeToDestroy)
            {
                timeToDestroy = Time.time + 10;
                Destroy(gameObject);
            }
        }

    }
}
