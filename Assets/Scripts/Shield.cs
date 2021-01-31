using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    Transform activeCharacter;
    public float maxShieldHP;
    public float currentShieldHP;
    public ShieldUI healthbar;

    private float _currentScale = InitScale;
    private const float TargetScale = 7.4f;
    private const float InitScale = 0.1f;
    private const int FramesCount = 10;
    private const float AnimationTimeSeconds = 0.1f;
    private float _deltaTime = AnimationTimeSeconds / FramesCount;
    private float _dx = (TargetScale - InitScale) / FramesCount;
    private bool _upScale = true;

    public GameObject destroyParticles;

    private void Start()
    {
        currentShieldHP = maxShieldHP;
        healthbar.SetMaxHealth(maxShieldHP);
        StartCoroutine(Grow());
    }

    void Update()
    {
        activeCharacter = FindObjectOfType<characterSwitch>().activeCharacter.transform;
        transform.position = activeCharacter.position;
        healthbar.SetHealth(currentShieldHP);
        if (currentShieldHP <= 0)
        {
            Instantiate(destroyParticles, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 13)
        {
            TakeDamage(collision.GetComponent<EnemyBullet>().bulletDamage);

        }
    }

    public void TakeDamage(int value)
    {
        var damage = 10 - value;
        currentShieldHP -= damage;
    }

    private IEnumerator Grow()
    {
        while (_upScale)
        {
            _currentScale += _dx;
            if (_currentScale > TargetScale)
            {
                _upScale = false;
                _currentScale = TargetScale;
            }
            transform.localScale = Vector3.one * _currentScale;
            yield return new WaitForSeconds(_deltaTime);
        }
    }
}
