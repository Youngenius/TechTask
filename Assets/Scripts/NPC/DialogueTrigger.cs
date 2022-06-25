using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    private DialogueManager _dialogueManager;
    [SerializeField] private Dialogue[] _dialogues;

    private int _counter = 0;

    private void Start()
    {
        _dialogueManager = GetComponent<DialogueManager>();
    }

    public void TriggerDialogue()
    {
        
        _dialogueManager.StartDialogue(_dialogues[_counter]);
        _counter++;
    }
}
