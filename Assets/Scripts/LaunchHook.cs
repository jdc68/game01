using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class LaunchHook : MonoBehaviour
{
    public PlayerMovementController playerController;
    public Transform firePoint;
    public GameObject hookPrefab;
    [HideInInspector]
    public bool hooked;
    public float hookCooldown;
    private float timeLeft;
    [HideInInspector]
    public bool onCooldown;
    public PressAbility ui;
    private GameObject hook;

    private void OnEnable()
    {
        if (hook != null)
        {
            Destroy(hook);
            hooked = false;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && !hooked & !onCooldown)
        {
            Hook();
        }

        if (hooked)
        {
            GetComponent<SpriteRenderer>().enabled = true;
            ui.Activate();
            playerController.ability = true;
            playerController.changeAnimationState(playerController.localActiveCharacter.ab);
        } else
        {
            GetComponent<SpriteRenderer>().enabled = false;
            ui.Deactivate();
            playerController.ability = false;
        }
    }

    public void Hook() 
    {
        ui.cooldownTime = hookCooldown;
        hooked = true;
        hook = Instantiate(hookPrefab, firePoint.position, firePoint.rotation, transform.parent);
    }

    public IEnumerator HookCooldown(float seconds)
    {
        timeLeft = seconds;
        float fillAmount = timeLeft / hookCooldown;
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
        StartCoroutine(HookCooldown(timeLeft));
    }

    public void StartCooldown()
    {
        StartCoroutine(HookCooldown(hookCooldown));
    }
}
