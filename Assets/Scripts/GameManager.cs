using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GameManager : MonoBehaviour
{
    public bool dead;
    public Canvas deathScreen;
    public Volume volume;
    private ColorAdjustments colorAdjustments;
    private DepthOfField depthOfField;
    public int coins;

    void Start()
    {
        volume = FindObjectOfType<Volume>();
        volume.profile.TryGet<ColorAdjustments>(out colorAdjustments);
        volume.profile.TryGet<DepthOfField>(out depthOfField);
    }

    void Update()
    {
        if (!dead)
            deathScreen.gameObject.SetActive(false);

    }

    void FixedUpdate()
    {
        if (dead)
        {
            colorAdjustments.saturation.value -= 2;
            depthOfField.focusDistance.value -= 0.03f;
            if (depthOfField.focusDistance.value <= 0.9f)
                depthOfField.focusDistance.value = 0.9f;
        } else
        {
            colorAdjustments.saturation.value = 0;
            depthOfField.focusDistance.value = 3.5f;
        }
    }

    public void GameOver()
    {
        if (!dead)
        {
            dead = true;
            deathScreen.gameObject.SetActive(true);
            deathScreen.GetComponent<ShowDeathScreen>().show();
        }   
    }

}
