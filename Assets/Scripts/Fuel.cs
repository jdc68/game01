using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class Fuel : MonoBehaviour
{
    public Slider slider;
    public TMP_Text value;

    public void SetMaxFuel(float fuel)
    {
        slider.maxValue = fuel;
        slider.value = fuel;

        string s = System.Math.Round(slider.value, 2).ToString();
        value.text = s;
    }

    public void SetFuel(float fuel)
    {
        slider.value = fuel;
        string s = System.Math.Round(slider.value, 2).ToString();
        value.text = s;
    }
}
