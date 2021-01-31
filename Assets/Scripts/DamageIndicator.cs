using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageIndicator : MonoBehaviour
{
    public static DamageIndicator Create(Vector3 position, float damage)
    {
        Transform damageIndicatorTransform = Instantiate(GameAssets.i.damageIndicator, position, Quaternion.identity);
        DamageIndicator damageIndicator = damageIndicatorTransform.GetComponent<DamageIndicator>();
        damageIndicator.Setup(damage);

        return damageIndicator;
    }

    private TextMeshPro textMesh;
    public float moveSpeed;
    public Vector3 moveSpeedVector; 
    public float disappearTimer;
    private Color textColor;
    private const float DISAPPEAR_TIMER_MAX = 1f;
    private static int sortingOrder;

    public void Awake()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
    }

    public void Setup(float damage)
    {
        textMesh.text = (-damage).ToString();
        textColor = textMesh.color;
        moveSpeedVector = new Vector3(1, 1) * moveSpeed;
        sortingOrder++;
        textMesh.sortingOrder = sortingOrder;
    }

    private void Update()
    {
        transform.position += moveSpeedVector * Time.deltaTime;
        moveSpeedVector -= moveSpeedVector * 8f * Time.deltaTime;

        if (disappearTimer > DISAPPEAR_TIMER_MAX * 5f)
        {
            // first half
            float increaseAmount = 1f;
            transform.localScale += Vector3.one * increaseAmount * Time.deltaTime;
        } else
        {
            //second half
            float decreaseAmount = 1f;
            transform.localScale -= Vector3.one * decreaseAmount * Time.deltaTime;
        }
        disappearTimer -= Time.deltaTime;
        if (disappearTimer < 0)
        {
            float disappearSpeed = 3f;
            textColor.a -= disappearSpeed * Time.deltaTime;
            textMesh.color = textColor;
            if (textColor.a <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
