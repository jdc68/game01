using Microsoft.Win32;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using UnityEngine;
using UnityEngine.Rendering;

public class Jetpack : MonoBehaviour
{
    public float thrustForce;
    public float maxJetSpeed;
    public float fuelLevel;
    public float maxFuel = 100f;
    public float fuelRate = 0.25f;
    public float rechargeRate = 0.5f;
    public float RechargeCD = 3f;
    private bool jetpackActive = true;
    private float nextConsumption;
    private float timeToRecharge;
    public bool thrust;
    public bool infiniteFuel = false;

    public Fuel fuel;
    private Rigidbody2D rb;
    public ParticleSystem jetSmoke;
    public ParticleSystem burstSmoke;
    public GameObject jetLight;
    PlayerMovementController player;

    // Start is called before the first frame update
    void Start()
    {
        player = transform.parent.GetChild(0).GetComponent<PlayerMovementController>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (infiniteFuel)
        {
            fuelLevel = maxFuel - 1;
        }
        if (Input.GetKey(KeyCode.LeftShift) && fuelLevel > 0 && jetpackActive)
        {
            thrust = true;
        }
        else if (fuelLevel == 0 || !jetpackActive)
        {
            thrust = false;
        }

        if ((!Input.GetKey(KeyCode.LeftShift)) && fuelLevel < maxFuel && jetpackActive)
        {
            thrust = false;
            if (Time.time > timeToRecharge)
            {
                timeToRecharge = Time.time + rechargeRate;
                fuelLevel++;
            }
        }
    }

    private void FixedUpdate()
    {
        if (thrust)
        {
            jetSmoke.Emit(1);
            jetLight.SetActive(true);
            FindObjectOfType<AudioManager>().GetComponents<AudioSource>()[13].enabled = true;
            if (!FindObjectOfType<AudioManager>().GetComponents<AudioSource>()[13].isPlaying)
                FindObjectOfType<AudioManager>().Play("Jet");

            player.changeAnimationState(player.localActiveCharacter.idle);
            rb.AddForce(transform.up * thrustForce);
            if (rb.velocity.y > maxJetSpeed)
            {
                rb.velocity = new Vector2(rb.velocity.x, maxJetSpeed);
            }

            if (Time.time > nextConsumption)
            {
                nextConsumption = Time.time + fuelRate;
                fuelLevel -= 1f;
            }
        } else
        {
            jetLight.SetActive(false);
            FindObjectOfType<AudioManager>().GetComponents<AudioSource>()[13].enabled = false;
        }

        fuel.SetFuel(fuelLevel);
        player.thrust = thrust;
    }
}
