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
                if (!FindObjectOfType<GameManager>().phoneWasCalled && dialogue.phoneCall)
                {
                    FindObjectOfType<GameManager>().phoneWasCalled = true;
                    FindObjectOfType<GameManager>().dialogue = dialogue;
                    FindObjectOfType<Phone>().GetComponent<Animator>().SetBool("phoneUp", true);
                    FindObjectOfType<Phone>().calling = true;
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
