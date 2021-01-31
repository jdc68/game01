using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : MonoBehaviour
{
    public int grade = 10;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();
            try
            {
                FindObjectOfType<AudioManager>().Play("Homework");
                player.TakeDamage(grade);
                Destroy(gameObject);
              
            } catch { }
            

            
        }
    }
}
