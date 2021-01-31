using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewEnemy : MonoBehaviour
{
    public float speed;
    public float stoppingDistance;
    public float retreatDistance;
    public float timeToFire;
    public float fireRate;
    public float range;
    public float maxHealth;
    public float currentHealth;

    public EnemyBullet bullet;
    public LayerMask playerLayer;
    public Transform firePoint;
    public Healthbar_enemy healthbar;
    public EnemyArm arm;
    public ParticleSystem particles;
    public GameObject loot;

    private Rigidbody2D player;
    private Animator animator;

    private void Awake()
    {
        try
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        }
        catch { }
    }
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthbar.SetMaxHealth(maxHealth);
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!player)
        {
            try
            {
                player = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
            }
            catch { }
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        try
        {
            if (Vector2.Distance(transform.position, player.position) <= range)
            {
                if (player.position.x > transform.position.x)
                    transform.eulerAngles = new Vector3(0f, 180f, 0f);
                else
                    transform.eulerAngles = new Vector3(0f, 0f, 0f);


                RaycastHit2D hit = Physics2D.Raycast(firePoint.position, (player.transform.position - firePoint.position), range);
                if (hit.transform.tag == "Player")
                {
                    if (Vector2.Distance(transform.position, player.position) > stoppingDistance)
                    {
                        animator.SetBool("moving", true);
                        transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
                    }
                    else if (Vector2.Distance(transform.position, player.position) > retreatDistance && Vector2.Distance(transform.position, player.position) < stoppingDistance)
                    {
                        animator.SetBool("moving", false);
                        transform.position = this.transform.position;

                    }
                    else if (Vector2.Distance(transform.position, player.position) < retreatDistance)
                    {
                        animator.SetBool("moving", true);
                        transform.position = Vector2.MoveTowards(transform.position, player.position, -speed * Time.deltaTime);
                    }
                    if (Time.time > timeToFire)
                    {
                        Instantiate(bullet, firePoint.position, Quaternion.identity);
                        timeToFire = Time.time + 1 / fireRate;
                    }
                }
                else
                    animator.SetBool("moving", false);
            }
        }
        catch { }
        transform.rotation = new Quaternion(0f, transform.rotation.y, 0f, 0f);
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        healthbar.SetHealth(currentHealth);
        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        Instantiate(particles, transform.position, Quaternion.identity);
        Instantiate(loot, transform.position, Quaternion.identity);
        Destroy(transform.parent.gameObject);
    }
}
