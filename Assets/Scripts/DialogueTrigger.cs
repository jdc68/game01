using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public bool triggered;

    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision & collision.tag == "Player")
        {
            if (!triggered)
            {
                triggered = true;
                if (!PhoneManager.Instance.phoneWasCalled && dialogue.phoneCall)
                {
                    PhoneManager.Instance.phoneWasCalled = true;
                    PhoneManager.Instance.nextDialogue = dialogue;
                    PhoneManager.Instance.phoneUp = true;
                    PhoneManager.Instance.calling = true;
                } else
                {
                    TriggerDialogue();
                }
            } else
            {
                return;
            } 
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, "_Popup");
    }
}
