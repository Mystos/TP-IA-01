using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;

    public GameObject dialogueBox;


    public void TriggerDialogue()
    {
        if (!dialogueBox.gameObject.activeSelf)
        {
            dialogueBox.gameObject.SetActive(true);
        }
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }
}
