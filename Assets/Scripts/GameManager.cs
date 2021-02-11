using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GameManager : MonoBehaviour
{
    //public bool dead;
    public Canvas deathScreen;
    public Volume volume;
    private ColorAdjustments colorAdjustments;
    private DepthOfField depthOfField;
    public bool phoneWasCalled;
    public Dialogue dialogue;

    void Start()
    {
        volume = FindObjectOfType<Volume>();
        volume.profile.TryGet<ColorAdjustments>(out colorAdjustments);
        volume.profile.TryGet<DepthOfField>(out depthOfField);
    }

    void Update()
    {
        if (!ScoreManager.Instance.isDead)
            deathScreen.gameObject.SetActive(false);
    }

    void FixedUpdate()
    {
        if (ScoreManager.Instance.isDead)
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
        if (!ScoreManager.Instance.isDead)
        {
            ScoreManager.Instance.isDead = true;
            deathScreen.gameObject.SetActive(true);
            deathScreen.GetComponent<ShowDeathScreen>().show();
        }   
    }

    public void AcceptCall()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
        //FindObjectOfType<Phone>().calling = false;
        PhoneManager.Instance.calling = false;
        FindObjectOfType<AudioManager>().Play("beep");
    }

}
