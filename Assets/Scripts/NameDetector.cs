using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NameDetector : MonoBehaviour
{
    public TextMeshProUGUI nameLabel;

    void Awake()
    {
        nameLabel.text = gameObject.name;
    }
}