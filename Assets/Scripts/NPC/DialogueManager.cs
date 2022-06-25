using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    private CameraZoom _camera;

    private Queue<string> sentences;

    public List<GameObject> _textAreas;
    public List<TMP_Text> _dialogueTexts;

    private int _counter = 0;
    private bool _blockDialogueInput = true;

    public UnityEvent OnFinishedSpeakingEvent;

    void Start()
    {
        sentences = new Queue<string>();
        _camera = GameObject.FindGameObjectWithTag("Camera").GetComponent<CameraZoom>();
    }

    private void Update()
    {
        if (!_blockDialogueInput && _counter < _dialogueTexts.Count)
           if (Input.GetKeyDown(KeyCode.Space))
           {
              DisplayNextSentence();
           }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        //Block camera input so it won't zoom until dialogue is finished
        _camera.BlockCamInput = true;
        sentences.Clear();

        foreach (string replic in dialogue.replics)
        {
            sentences.Enqueue(replic);
        }

        Debug.Log(sentences.Count);
        _textAreas[_counter].SetActive(true);
        Debug.Log(_textAreas.Count);

        DisplayNextSentence();
    }

    private void DisplayNextSentence()
    {  
        _blockDialogueInput = true;
        if (sentences.Count == 0)
        {
            EndDialogue();
            _camera.BlockCamInput = false;
            return;
        }

        
        string sentence = sentences.Dequeue();

        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));  
    }

    private void EndDialogue()
    {
        _textAreas[_counter].SetActive(false);

        _counter++;
    }

    private IEnumerator TypeSentence(string sentence)
    {
        float timeBetweenLetters = 0.07f;

        _dialogueTexts[_counter].text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            _dialogueTexts[_counter].text += letter;
            yield return new WaitForSeconds(timeBetweenLetters);
            yield return null;
        }

        _blockDialogueInput = false;
    }
}
