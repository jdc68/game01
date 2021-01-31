using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Lava : MonoBehaviour
{
    public int lavaDamage;
    public float lavaDamageEnemy;
    public ParticleSystem splash;

    private bool takingDamage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            rb.drag = 30f;
        }
        //takingDamage = true;
        if (collision)
        {
            if (collision.tag == "enemy")
            {
                if (collision.GetComponent<EnemyHP>())
                {
                    EnemyHP enemy = collision.GetComponent<EnemyHP>();
                    enemy.repeatingDamageAmount = lavaDamageEnemy;
                    enemy.damageRate = .5f;
                    enemy.takingDamage = true;
                    DamageIndicator.Create(collision.transform.position, lavaDamageEnemy);
                }
            }
            StartCoroutine(DealDamage(collision.gameObject));
        }

        Instantiate(splash, collision.transform.position, Quaternion.Euler(-90, 0, 0));
    }

    private void OnTriggerExit2D(Collider2D collision)
    { 
        if (collision.transform.tag == "Player")
            collision.GetComponent<Rigidbody2D>().drag = 0f; ;

        if (collision.tag == "enemy")
        {
            if (collision.GetComponent<EnemyHP>())
            {
                collision.GetComponent<EnemyHP>().takingDamage = false;
            }
        }

        //takingDamage = false;
        StopCoroutine(DealDamage(collision.gameObject));
    }

    IEnumerator DealDamage(GameObject target)
    {
        while (true)
        {
            if (target != null) 
            {
                if (target.tag == "Player")
                    target.GetComponent<Player>().TakeDamage(lavaDamage);
            }

            
            yield return new WaitForSeconds(.5f);
        }
        
    }
}
