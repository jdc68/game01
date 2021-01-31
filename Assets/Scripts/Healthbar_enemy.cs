using UnityEngine;
using UnityEngine.UI;

public class Healthbar_enemy : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    private void Update()
    {
        transform.rotation = new Quaternion(0, 0, 0, 0);
    }

    public void SetMaxHealth(float health)
    {
        slider.maxValue = health;
        slider.value = health;

        fill.color = gradient.Evaluate(1f);
    }
    // Start is called before the first frame update
    public void SetHealth(float health)
    {
        slider.value = health;

        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
