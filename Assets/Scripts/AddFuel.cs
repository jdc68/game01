using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddFuel : MonoBehaviour
{
    public float fuelToAdd;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            characterSwitch activeCharacter = FindObjectOfType<characterSwitch>();
            if (activeCharacter.activeCharacterIndex == 0)
            {
                Jetpack jetpack = other.transform.GetComponent<Jetpack>();
                if (jetpack.fuelLevel < 100)
                {
                    FindObjectOfType<AudioManager>().Play("Smokes");
                    jetpack.fuelLevel = Mathf.Clamp(jetpack.fuelLevel + fuelToAdd, 0, 100);
                    Destroy(gameObject);
                }
            }
        }
    }
}
