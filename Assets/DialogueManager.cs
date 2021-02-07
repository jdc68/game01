using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text dialogueText;
    public float textSpeed;

    public Queue<string> sentences;
    Queue<Sound> voiceLines;

    private void Start()
    {
        sentences = new Queue<string>();
        voiceLines = new Queue<Sound>();
    }  

    public void StartDialogue(Dialogue dialogue)
    {
        if (dialogue.phoneCall)
            FindObjectOfType<Phone>().GetComponent<Animator>().SetBool("phoneUp", true);

        Color color = new Color(0, 0, 0, .4f);
        nameText.transform.GetComponentInParent<Image>().color = color;
        nameText.text = dialogue.name;

        sentences.Clear();
        voiceLines.Clear();

        foreach (VoiceLine line in dialogue.lines)
        {
            line.audio.source = gameObject.AddComponent<AudioSource>();
            line.audio.source.clip = line.audio.clip;

            line.audio.source.volume = line.audio.volume;
            line.audio.source.pitch = line.audio.pitch;
            line.audio.source.loop = line.audio.loop;

            sentences.Enqueue(line.sentence);
            voiceLines.Enqueue(line.audio);
        }

        DisplayNextSentence();
        
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            Invoke("EndDialogue", 2);
            return;
        }

        string sentence = sentences.Dequeue();
        Sound voiceLine = voiceLines.Dequeue();
        voiceLine.source.Play();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentece)
    {
        dialogueText.text = "";
        foreach (char letter in sentece.ToCharArray())
        {
            dialogueText.text += letter;
            if (dialogueText.text == sentece)
            {
                Invoke("DisplayNextSentence", 1);
            }
            yield return new WaitForSeconds(textSpeed);
        }
    }

    public void EndDialogue()
    {
        Color color = new Color(0, 0, 0, 0);
        nameText.transform.GetComponentInParent<Image>().color = color;
        nameText.text = "";
        dialogueText.text = "";
        FindObjectOfType<Phone>().GetComponent<Animator>().SetBool("phoneUp", false);
        Debug.Log("end of conversation");
    }
}
