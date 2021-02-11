using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PressAbility : MonoBehaviour
{
    public bool instant;
    public bool controlledByScript;
    public Sprite inactiveSprite;
    public Sprite activeSprite;
    public KeyCode key;
    private Image image;
    public float activeForSeconds;
    public TMP_Text Cooldown;
    public bool onCooldown;
    public Image abilityTimer;
    public float cooldownTime;

    private void Start()
    {
        image = GetComponent<Image>();
        image.sprite = inactiveSprite;
        try
        {
            abilityTimer.fillAmount = 0;
        }
        catch { }
    }

    // Update is called once per frame
    void Update()
    {
        if (onCooldown)
        {
            abilityTimer.fillAmount -= 1f / cooldownTime * Time.deltaTime;

            if(abilityTimer.fillAmount <= 0)
            {
                abilityTimer.fillAmount = 0;
            }
            
        }
        if (!controlledByScript)
        {
            if (instant && !ScoreManager.Instance.isDead)
            {
                if (Input.GetKeyDown(key))
                {
                    Activate();
                }
                if (Input.GetKeyUp(key))
                {
                    Deactivate();
                }
            } else
            {
                if (Input.GetKey(key) && !onCooldown && !ScoreManager.Instance.isDead)
                {
                    StartCoroutine(changeSprite());
                }
            }
        }
    }

    public void Activate()
    {
        image.sprite = activeSprite;
    }

    public void Deactivate()
    {
        image.sprite = inactiveSprite;
    }

    public void StartCooldown(float seconds, float fillAmount)
    {
        StartCoroutine(CooldownTimer(seconds, fillAmount));
    }


    IEnumerator CooldownTimer(float seconds, float fillAmount) 
    {
        abilityTimer.fillAmount = fillAmount;
        onCooldown = true;
        float timeLeft = seconds;
        Cooldown.text = timeLeft.ToString();
        Cooldown.gameObject.SetActive(true);
        while (timeLeft > 0)
        {
            yield return new WaitForSeconds(.1f);
            timeLeft -= .1f;
            timeLeft = Mathf.Round(timeLeft * 10f) * 0.1f;
            Cooldown.text = timeLeft.ToString();
        }
        if (timeLeft <= 0)
        {
            onCooldown = false;
            Cooldown.gameObject.SetActive(false);
        }
    }

    IEnumerator changeSprite()
    {
        image.sprite = activeSprite;
        yield return new WaitForSeconds(activeForSeconds);
        image.sprite = inactiveSprite;
    }
}
