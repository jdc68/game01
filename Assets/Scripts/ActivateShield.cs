using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateShield : MonoBehaviour
{
    public GameObject shield;
    public PressAbility ui;
    public bool shieldActive;
    public GameObject shieldInstance;
    public bool onCooldown;
    public float cooldown;
    private float timeLeft;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !shieldActive && !onCooldown)
        {
            shieldActive = true;
            shieldInstance = Instantiate(shield, transform.position, Quaternion.identity, transform.parent.parent);
            ui.Activate();
            ui.cooldownTime = cooldown;
        }

        if (!shieldActive)
        {
            ui.Deactivate();
        }

        if (!shieldInstance && shieldActive)
        {
            shieldActive = false;
            onCooldown = true;
            StartCooldown();
        }
    }

    public IEnumerator ShieldCooldown(float seconds)
    {
        timeLeft = seconds;
        float fillAmount = timeLeft / cooldown;
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

    public void ResumeCooldown()
    {
        StartCoroutine(ShieldCooldown(timeLeft));
    }

    public void StartCooldown()
    {
        StartCoroutine(ShieldCooldown(cooldown));
    }
}
