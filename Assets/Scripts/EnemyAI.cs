using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;


public class EnemyAI : MonoBehaviour
{
    public Transform target;
    public Transform groundCheck;
    public Transform groundCheck1;


    public float activateDistance = 50f;

    public float speed = 200f;
    public float nextWaypointDistance = 3f;
    public float jumpNodeHeight = 0.8f;
    public float jumpModifier = 0.3f;
    public float jumpCheckOffset = 0.1f;
    public LayerMask playerMask;

    public float playerDetectionRadius;
    public float attackDelay = .5f;
    [Range(2, 10)]
    public int attackValue = 4;

    public bool followEnabled = true;
    public bool jumpEnabled = true;
    bool attacked;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;
    public bool isGrounded = false;

    Seeker seeker;
    Rigidbody2D rb;

    public Transform enemyGFX;


    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        target = FindObjectOfType<Player>().transform;
        InvokeRepeating("UpdatePath", .5f, .5f);
    }

    void UpdatePath()
    { 
        if (seeker.IsDone() && target)
            seeker.StartPath(rb.position, target.position, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (FindObjectOfType<Player>())
            target = FindObjectOfType<Player>().transform;
        if (TargetInRange() && followEnabled)
        {
            PathFollow();
        }

        Collider2D playerInSight = Physics2D.OverlapCircle(rb.position, playerDetectionRadius, playerMask);
        if (playerInSight && !attacked)
        {
            followEnabled = false;
            rb.velocity = Vector2.zero;
            Invoke("DamagePlayer", attackDelay);
        }
    }

    void DamagePlayer()
    {
        attacked = true;
        target.GetComponent<Player>().TakeDamage(attackValue);
        Destroy(transform.parent.gameObject);

    }

    void PathFollow()
    {
        if (path == null)
            return;

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        // Ground Check
        int mask = 1 << LayerMask.NameToLayer("Ground");
        isGrounded = Physics2D.Linecast(transform.position, groundCheck.position, mask) || Physics2D.Linecast(transform.position, groundCheck1.position, mask);

        // Direction Calculator
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        // Jump
        if (jumpEnabled && isGrounded)
        {
            if(direction.y > jumpNodeHeight)
            {
                rb.AddForce(Vector2.up * jumpModifier * Time.deltaTime, ForceMode2D.Impulse);
            }
        }

        // Movement
        if (!isGrounded) 
            force.y = 0;
        rb.AddForce(force, ForceMode2D.Impulse);

        // Next waypoint
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        // GFX Direction
        if (rb.velocity.x >= 0.01f)
        {
            enemyGFX.localScale = new Vector3(-Mathf.Abs(enemyGFX.localScale.x), enemyGFX.localScale.y, enemyGFX.localScale.z);
        }
        else if (rb.velocity.x <= -0.01f)
        {
            enemyGFX.localScale = new Vector3(Mathf.Abs(enemyGFX.localScale.x), enemyGFX.localScale.y, enemyGFX.localScale.z);
        }
    }

    private bool TargetInRange()
    {
        return Vector2.Distance(transform.position, target.position) <= activateDistance;
    }

    private void OnDrawGizmos()
    {
        Ray ray = new Ray(transform.position, -Vector3.up * (GetComponent<Collider2D>().bounds.extents.y + jumpCheckOffset));
        Gizmos.DrawRay(ray);

        Gizmos.DrawWireSphere(transform.position, playerDetectionRadius);
    }
}
