using UnityEngine;
using UnityEngine.UIElements;
using TMPro;

public class EnemyBullet : MonoBehaviour
{
    public Rigidbody2D rb;
    private GameObject player;
    public float bulletSpeed = 1f;
    public int bulletDamage;
    public bool deflected = false;
    public GameObject destroyParticles;
    private float distanceToFly = 1000f;
    float distanceTraveled = 0f;
    Vector2 targetDirection;
    public TMP_Text text;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        targetDirection = player.transform.position - transform.position;
        float distance = targetDirection.magnitude;
        Vector2 direction = targetDirection / distance;
        direction.Normalize();
        rb.AddForce(direction * bulletSpeed);
        text.text = bulletDamage.ToString();
        Physics2D.IgnoreLayerCollision(13, 15);
        Physics2D.IgnoreLayerCollision(13, 20);
        Physics2D.IgnoreLayerCollision(13, 21);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!deflected)
        {
            Player character = other.GetComponent<Player>();
            try
            {
                character.TakeDamage(bulletDamage);
            }
            catch { }
        }
        else
        {
            EnemyHP character = other.GetComponent<EnemyHP>();
            try
            {
                character.TakeDamage(bulletDamage);
                DamageIndicator.Create(character.transform.position, bulletDamage);
            }
            catch { }
        }
        Instantiate(destroyParticles, transform.position, Quaternion.identity);
        Destroy(gameObject);

    }

    private void FixedUpdate()
    {
        if (deflected)
        {
            gameObject.layer = 19;
            Physics2D.IgnoreLayerCollision(19, 21);
        }
        distanceTraveled++;
        if (distanceTraveled >= distanceToFly)
        {
            Destroy(gameObject);
        }
    }
}
