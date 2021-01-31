using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Healthbar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public TMP_Text hp;

    public void SetMaxHealth(float health)
    {
        slider.maxValue = health;
        slider.value = health;

        fill.color = gradient.Evaluate(1f);

        // hp.text = (slider.normalizedValue * 100).ToString() + "%";
        hp.text = System.Math.Round(slider.value, 2).ToString();
    }
    // Start is called before the first frame update
    public void SetHealth(float health)
    {
        slider.value = health;

        fill.color = gradient.Evaluate(slider.normalizedValue);

        // hp.text = (slider.normalizedValue * 100).ToString() + "%";
        hp.text = System.Math.Round(slider.value, 2).ToString();
    }
}
