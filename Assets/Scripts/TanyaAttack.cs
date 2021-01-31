using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class TanyaAttack : MonoBehaviour
{
    public PlayerMovementController player;
    private bool isAttackPressed;
    private bool isAttacking;
    public float damage = 7f;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    private float timeToAttack = 0f;
    [SerializeField]
    private float attackDelay;

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && !transform.GetComponent<LaunchHook>().hooked)
        {
            if (Time.time >= timeToAttack)
            {
                Attack();
                timeToAttack = Time.time + attackDelay;
            }
        }

        if (isAttackPressed)
        {
            isAttackPressed = false;

            if (!isAttacking)
            {
                isAttacking = true;
                player.changeAnimationState(player.localActiveCharacter.hit);
            }

            Invoke("AttackComplete", attackDelay);

        }
        player.isAttacking = isAttacking;
    }
    void AttackComplete()
    {
        isAttacking = false;
        player.changeAnimationState(player.localActiveCharacter.idle);
    }

    void Attack()
    {
        isAttackPressed = true;

        Collider2D target = Physics2D.OverlapCircle(attackPoint.position, attackRange, enemyLayers);
        EnemyHP enemy = null;
        try
        {
            enemy = target.GetComponent<EnemyHP>();
        }
        catch { }
        if (enemy != null)
        {
            enemy.GetComponent<EnemyHP>().TakeDamage(damage);
            DamageIndicator.Create(enemy.transform.position, damage);
            int attackIndex = Random.Range(1, 4);
            string soundClip = string.Concat("Swidle", attackIndex.ToString());
            FindObjectOfType<AudioManager>().Play(soundClip);
        } else
        {
            int attackIndex = Random.Range(1, 4);
            string soundClip = string.Concat("Swidle", attackIndex.ToString());
            FindObjectOfType<AudioManager>().Play(soundClip);
        }
    }

    private void OnDrawGizmos()
    {
        if (attackPoint == null) return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
