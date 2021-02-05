using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Genji : MonoBehaviour
{
    public PlayerMovementController player;
    public Rigidbody2D dashPoint;
    [SerializeField] private float dashForce;
    [SerializeField] private float dashDuration;
    private Rigidbody2D rb;
    public BoxCollider2D mainCollider;
    private bool dashing = false;
    private Vector2 closesetTarget;
    public LayerMask layerMask1;
    private GameObject enemyTarget;
    public float dashDamage;
    int dash = 0;
    RaycastHit2D[] hitEnemies;
    public PressAbility ui_1;
    public PressAbility ui_2;
    public float dashCooldown;
    public bool onCooldown = false;
    public float timeLeft;
    public ParticleSystem dustPS;

    private BoxCollider2D box;
    private CapsuleCollider2D capsule;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
        capsule = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(rb.position, dashPoint.position - rb.position, Vector2.Distance(dashPoint.position, rb.position), layerMask1);
        hitEnemies = Physics2D.RaycastAll(rb.position, dashPoint.position - rb.position, Vector2.Distance(dashPoint.position, rb.position), player.enemyMask);
        if (hit && hit.collider != null)
            closesetTarget = hit.point;
        else 
            closesetTarget = dashPoint.position;

        if (Input.GetKeyDown(KeyCode.LeftShift) && !onCooldown && !FindObjectOfType<GameManager>().dead)
        {
            onCooldown = true;
            dashing = true;
            StartCoroutine(Cast(closesetTarget));
            dash++;
            if (dash % 2 == 0)
            {
                FindObjectOfType<AudioManager>().Play("Dash1");
            } else
            {
                FindObjectOfType<AudioManager>().Play("Dash2");
            }
        }

        if (Input.GetButtonDown("Jump") && player.canDoubleJump && !player.grounded)
        {
            ui_1.Activate();
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.velocity = new Vector2(rb.velocity.x, player.jumpSpeed / 1.5f);
            player.doubleJumping = true;
            player.canDoubleJump = false;
            dustPS.Play();
        }
        if (player.grounded)
        {
            ui_1.Deactivate();
        }
    }

    private void FixedUpdate()
    {
        if (dashing)
        {
            box.enabled = false;
            capsule.enabled = true;

            if (hitEnemies.Length > 0)
            {
                foreach (RaycastHit2D enemy in hitEnemies)
                {
                    if (enemy.transform.tag != "enemy") break;
                    else if (Vector2.Distance(transform.position, enemy.transform.position) <= Vector2.Distance(transform.position, closesetTarget))
                    {
                        enemy.transform.GetComponent<EnemyHP>().TakeDamage(dashDamage);
                        DamageIndicator.Create(enemy.transform.position, dashDamage);
                    }
                }
            }

            onCooldown = true;
            dashing = false;
        }
    }

    private IEnumerator Cast(Vector2 target)
    {
        ui_2.Activate();
        player.groundCheck.gameObject.SetActive(false);
        player.ability = true;
        rb.gameObject.GetComponent<TrailRenderer>().emitting = true;
        rb.AddForce((target - GetComponent<Rigidbody2D>().position) * dashForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(dashDuration);
        rb.velocity = Vector2.zero;
        rb.gameObject.GetComponent<TrailRenderer>().emitting = false;
        player.ability = false;
        player.groundCheck.gameObject.SetActive(true);
        ui_2.Deactivate();
        StartCoroutine(DashCooldown(dashCooldown));

    }

    private IEnumerator DashCooldown(float seconds)
    {
        timeLeft = seconds;
        ui_2.cooldownTime = dashCooldown;
        float fillAmount = timeLeft / dashCooldown;
        ui_2.StartCooldown(timeLeft, fillAmount);
        while (timeLeft > 0)
        {
            yield return new WaitForSeconds(.1f);
            timeLeft -= .1f;
            timeLeft = Mathf.Round(timeLeft * 10f) * 0.1f;
        }
        if (timeLeft <= 0)
        {
            onCooldown = false;
        }
    }

    public void ResumeCooldown()
    {
        StartCoroutine(DashCooldown(timeLeft));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "enemy")
        {
            Physics2D.IgnoreCollision(capsule, collision.collider);
            Physics2D.IgnoreCollision(box, collision.collider);

        }
    }

    private void OnDrawGizmos()
    {
        if (hitEnemies != null && hitEnemies.Length > 0)
        {
            foreach (RaycastHit2D enemy in hitEnemies)
            {
                try
                {
                    Gizmos.DrawWireCube(enemy.transform.position, Vector3.one);
                } catch { }
            }
        }
        if (enemyTarget)
        {
            Gizmos.DrawWireCube(enemyTarget.transform.position, Vector3.one);
        }
        Gizmos.DrawWireCube(closesetTarget, Vector3.one);
        Gizmos.DrawLine(player.transform.position, dashPoint.position);
    }
}
