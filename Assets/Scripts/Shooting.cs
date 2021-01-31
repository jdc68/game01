using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Shooting : MonoBehaviour
{
    public Transform firePoint;

    public Sprite blaster;
    public Sprite bow;

    public GameObject bulletPrefab;
    public float timeToShoot;
    public float fireRate = 0.5f;
    private double nextFire = 0.1;
    public float bulletForce = 10f;

    public GameObject defaultArrow;
    public GameObject sonicArrow;
    public float launchForce;
    public int maxArrows;
    [HideInInspector]
    public bool sonic;
    public float sonicDuration;
    public float sonicCooldown;
    [HideInInspector]
    public float sonicLeft;
    [HideInInspector]
    public bool onCooldown;
    public PressAbility ui;
    private float timeLeft;

    List<GameObject> arrows = new List<GameObject>();


    //private bool using_bow = true;

    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = bow;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1") & Time.time > nextFire)
        {
            if (sonic)
            {
                nextFire = Time.time + fireRate / 3;
                ShootBow(sonicArrow);
            }
            else
            {
                nextFire = Time.time + fireRate;
                ShootBow(defaultArrow);
            }
        }

        if (Input.GetKeyDown("e") && !sonic && !onCooldown)
        {
            StartCoroutine(SonicArrows(sonicDuration));
        }
    }


    //void ShootBlaster()
    //{
    //    FindObjectOfType<AudioManager>().Play("BlasterShoot");

    //    GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    //    Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
    //    rb.AddForce(firePoint.right * bulletForce, ForceMode2D.Impulse);
    //}

    void ShootBow(GameObject arrow)
    {
        FindObjectOfType<AudioManager>().Play("Arrow");

        GameObject newArrow = Instantiate(arrow, firePoint.position, firePoint.rotation);
        newArrow.GetComponent<Rigidbody2D>().velocity = transform.right * launchForce;
        arrows.Insert(0, newArrow);
        if (arrows.Count > maxArrows)
        {
            GameObject arrowToRemove = arrows[arrows.Count - 1];
            Destroy(arrowToRemove);
            arrows.RemoveAt(arrows.Count - 1);
        }
    }

    IEnumerator SonicArrows(float seconds)
    {
        sonic = true;
        sonicLeft = seconds;
        ui.Activate();
        while (sonicLeft > 0)
        {
            yield return new WaitForSeconds(.1f);
            sonicLeft -= .1f;
            sonicLeft = Mathf.Round(sonicLeft * 10f) * 0.1f;
        }
        if (timeLeft <= 0)
        {
            sonic = false;
            ui.Deactivate();
        }
        onCooldown = true;
        ui.cooldownTime = sonicCooldown;
        yield return StartCoroutine(SonicArrowsCooldown(sonicCooldown));
    }

    IEnumerator SonicArrowsCooldown(float seconds)
    {
        timeLeft = seconds;
        float fillAmount = timeLeft / sonicCooldown;
        ui.StartCooldown(timeLeft, fillAmount);
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

    public void ResumeSonic()
    {
        StartCoroutine(SonicArrows(sonicLeft));
    }

    public void ResumeCooldown()
    {
        StartCoroutine(SonicArrowsCooldown(timeLeft));
    }
}
