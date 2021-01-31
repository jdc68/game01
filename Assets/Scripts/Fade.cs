using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{
    Material material;
    bool fadingIn = false;
    float fade = 0f;
    [SerializeField]
    float fadeSpeed = 2.4f;


    void Start()
    {
        material = GetComponent<SpriteRenderer>().material;
        Invoke("FadeIn", 1);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (fadingIn)
            {
                fadingIn = false;
            }
            else
            {
                fadingIn = true;
            }
        }

        if (fadingIn)
        {
            fade = Mathf.Clamp(fade + Time.deltaTime * fadeSpeed, 0, 1);
        }
        else
        {
            fade = Mathf.Clamp(fade - Time.deltaTime * fadeSpeed, 0, 1);
        }
        material.SetFloat("_Fade", fade);

    }

    void FadeIn()
    {
        fadingIn = true;   
    }
}
