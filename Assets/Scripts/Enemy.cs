using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class Enemy : MonoBehaviour
{
    public float speed, stoppingDistance, retreatDistance, timeToFire, fireRate, range, maxHealth, currentHealth;
    public Transform point1, point2;

    public EnemyBullet bullet;
    public LayerMask layerMask;
    public Transform firePoint;
    public Transform nextPos;
    public Rigidbody2D player;
    public bool playerNearby;
    [Range(1, 10)]
    public int minGrade;
    [Range(1, 10)]
    private int maxGrade;
    public bool patrol;
    

    private Rigidbody2D rb;
    private Animator animator;


    private void Awake()
    {
        try
        {
            player = FindObjectOfType<PlayerMovementController>().transform.GetComponent<Rigidbody2D>();
        } catch { }
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        rb.position = new Vector3(point1.position.x, rb.position.y);
        Physics2D.IgnoreLayerCollision(8, 8);
    }

    private void Update()
    {
        try
        {
            player = FindObjectOfType<PlayerMovementController>().transform.GetComponent<Rigidbody2D>();
        } catch { }
        RaycastHit2D hit;
        if (player != null)
        {
            hit = Physics2D.Raycast(firePoint.position, (player.transform.position - firePoint.position), range, layerMask);

            if (hit && hit.transform.tag == "Player")
            {

                playerNearby = true;
                // Attack

                // Rotating
                if (player.position.x > transform.position.x)
                    transform.eulerAngles = new Vector3(0f, 180f, 0f);
                else
                    transform.eulerAngles = new Vector3(0f, 0f, 0f);

                // Moving
                if (rb.velocity.magnitude > 0.1)
                {
                    rb.velocity = new Vector2(0, rb.velocity.y);
                }


                // Shooting
                Attack(minGrade, maxGrade);
            }
            else
            {
                playerNearby = false;
                // Patrol

                // Moving
                if (patrol)
                {
                    if (rb.position.x <= point1.position.x)
                    {
                        nextPos = point2;
                    }
                    if (rb.position.x >= point2.position.x)
                    {
                        nextPos = point1;
                    }

                    if (nextPos == point2)
                    {
                        rb.velocity = new Vector2(speed, rb.velocity.y);
                        animator.SetBool("moving", true);
                        transform.eulerAngles = new Vector3(0f, 180f, 0f);
                    }
                    else
                    {
                        rb.velocity = new Vector2(-speed, rb.velocity.y);
                        animator.SetBool("moving", true);
                        transform.eulerAngles = new Vector3(0f, 0f, 0f);
                    }
                }
            }
        }
        
        if (rb.velocity.magnitude < 0.1)
        {
            animator.SetBool("moving", false);
        }
    }

    void Attack(int min, int max)
    {
        if (Time.time > timeToFire)
        {
            EnemyBullet bulletInstance = Instantiate(bullet, firePoint.position, Quaternion.identity);
            float playerCurrentHealth = FindObjectOfType<Health>().currentHealth;
            bulletInstance.bulletDamage = Random.Range(min, Mathf.RoundToInt(playerCurrentHealth));
            timeToFire = Time.time + 1 / fireRate;
        }
    }
}
