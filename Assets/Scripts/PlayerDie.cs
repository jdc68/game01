using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDie : MonoBehaviour
{
    public Player player;

    // Update is called once per frame
    void Update()
    {
    }
    
    void Die()
    {
        Destroy(gameObject);
    }
}
