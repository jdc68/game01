using UnityEngine.UI;
using TMPro;
using UnityEngine;

[ExecuteInEditMode]
public class JumpCharge : MonoBehaviour
{
    public Slider slider;
    public RectTransform maxFill;
    private RectTransform parent;

    private void Awake()
    {
        parent = GetComponent<RectTransform>();
    }

    private void Update()
    {
        parent = GetComponent<RectTransform>();
        float markerPosX = parent.sizeDelta.x * FindObjectOfType<Tanya>().jumpChargeToStun;
        maxFill.localPosition = new Vector3(markerPosX, maxFill.localPosition.y, maxFill.localPosition.z);
        float markerWidthX = parent.sizeDelta.x - parent.sizeDelta.x * FindObjectOfType<Tanya>().jumpChargeToStun;
        maxFill.sizeDelta = new Vector2(markerWidthX, maxFill.sizeDelta.y);
    }

    public void SetMaxHeight(float value)
    {
        slider.maxValue = value;
        slider.value = value;

        string s = System.Math.Round(slider.value, 2).ToString();
    }

    public void SetHeight(float value)
    {
        slider.value = value;
        string s = System.Math.Round(slider.value, 2).ToString();
    }
}
