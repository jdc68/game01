using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            FindObjectOfType<AudioManager>().Play("Coin");
            //other.transform.parent.parent.GetComponent<Health>().coins++;
            Player.coins++;
            Debug.Log(Player.coins);

            Destroy(gameObject);
        }
    }
}
