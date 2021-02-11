using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneManager : AutoCleanupSingleton<PhoneManager>
{
    public bool phoneWasCalled;
    public Dialogue nextDialogue;
    public bool phoneUp = false;
    public bool calling = false;

    [SerializeField]public void AcceptCall()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(nextDialogue);
        calling = false;
        //FindObjectOfType<AudioManager>().Play("beep");
        AudioManager.Instance.Play("beep");
    }
}
