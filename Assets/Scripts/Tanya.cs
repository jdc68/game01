using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;
using JetBrains.Annotations;
using UnityEditor.Experimental.GraphView;

public class Tanya : MonoBehaviour
{
    public PlayerMovementController playerController;
    Player player;
    [HideInInspector]
    public bool landed;
    public bool chargingJump;
    public float jumpCharge = 0;
    private float jumpForce;
    [Range(0, 1)]
    public float jumpChargeToStun = 0.8f;
    public float jumpForceRate;
    public float stunRadius;
    public float stunDamage;
    public float stunHeight;
    public ParticleSystem landingSmoke;
    public ParticleSystem dustPS;

    public JumpCharge jumpChargeController;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerMovementController>();
        player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        landingSmoke.transform.position = playerController.groundCheck.position + new Vector3(0, 1.4f, 0);

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            playerController.GetComponent<Rigidbody2D>().velocity = new Vector2(0, playerController.GetComponent<Rigidbody2D>().velocity.y);
        }
        if (playerController.grounded)
        {
            if (!landed)
            {
                if (jumpForce > jumpChargeToStun)
                {
                    Collider2D[] enemies = Physics2D.OverlapAreaAll(new Vector2(transform.position.x - stunRadius, transform.position.y), new Vector2(transform.position.x + stunRadius, transform.position.y - transform.localScale.y / 2), playerController.enemyMask);
                    foreach (Collider2D enemy in enemies)
                    {
                        enemy.GetComponent<Rigidbody2D>().AddForce(Vector2.up * stunHeight);
                        enemy.GetComponent<EnemyHP>().TakeDamage(stunDamage);
                        DamageIndicator.Create(enemy.transform.position, stunDamage);
                    }
                    landingSmoke.Play();
                    FindObjectOfType<AudioManager>().Play("FallHard");
                    try
                    {
                        CameraShaker.Instance.ShakeOnce(player.magnitude * 3, player.roughness, player.fadein, player.fadeout);
                    } catch { }
                }
                jumpForce = 0;
                landed = true;
            }
        } else
        {
            landed = false;
        }

        if(Input.GetButtonDown("Jump"))
        {
            chargingJump = true;
        } 
        if (Input.GetButtonUp("Jump"))
        {
            jumpForce = jumpCharge;
            dustPS.Play();
            jumpCharge = 0;
            chargingJump = false;
            playerController.zoomMagnifier = 0;
        }

        playerController.jumpMultiplier = jumpCharge;
        jumpChargeController.SetHeight(jumpCharge);
    }

    private void FixedUpdate()
    {
        if (chargingJump)
        {
            jumpCharge = Mathf.Clamp(jumpCharge + jumpForceRate * Time.deltaTime, 0, 1);
            if (jumpCharge < 1)
            {
                playerController.zoomMagnifier = Mathf.Clamp(playerController.zoomMagnifier + 3.4f * Time.deltaTime, 0, 3f);
            }
        }
        if (jumpCharge == 1 && playerController.grounded)
        {
            try
            {
                CameraShaker.Instance.ShakeOnce(player.magnitude / 5, player.roughness, player.fadein, player.fadeout);
            }
            catch { }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(new Vector2(transform.position.x - stunRadius, transform.position.y), 0.2f);
        Gizmos.DrawWireSphere(new Vector2(transform.position.x + stunRadius, transform.position.y - transform.localScale.y / 2), 0.2f);
        ;
    }
}


