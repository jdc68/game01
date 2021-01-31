using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenjiAttack : MonoBehaviour
{
    public PlayerMovementController player;
    public bool isAttackPressed;
    public bool isAttacking;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public float damage = 7f;
    private float timeToAttack = 0f;
    public GameObject slashEffect;

    [SerializeField]
    private float attackDelay;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (Time.time >= timeToAttack)
            {
                Attack();
                timeToAttack = Time.time + attackDelay;
            }
        }
    }

    private void FixedUpdate()
    {
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
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach(Collider2D enemy in hitEnemies)
        {
            if (enemy.GetComponent<EnemyHP>())
            {
                Instantiate(slashEffect, enemy.transform.position, enemy.transform.rotation, enemy.transform);
                enemy.GetComponent<EnemyHP>().TakeDamage(damage);
                DamageIndicator.Create(enemy.transform.position, damage);

            } else if (enemy.GetComponent<EnemyBullet>())
            {
                Instantiate(slashEffect, enemy.transform.position, transform.rotation);
                FindObjectOfType<AudioManager>().Play("Deflect");
                enemy.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                enemy.GetComponent<Rigidbody2D>().AddForce(attackPoint.right * enemy.GetComponent<EnemyBullet>().bulletSpeed * 2);
                EnemyBullet enemyBullet = enemy.GetComponent<EnemyBullet>();
                enemyBullet.deflected = true;
                enemyBullet.bulletDamage = (int)damage;
            }
        }

        // Sound
        int attackIndex = Random.Range(1, 4);
        string soundClip = string.Concat("Blade", attackIndex.ToString());
        FindObjectOfType<AudioManager>().Play(soundClip);
    }

    private void OnDrawGizmos()
    {
        if (attackPoint == null) return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
