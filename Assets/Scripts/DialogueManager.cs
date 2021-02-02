using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class DialogueManager : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text dialogueText;
    bool skip = false;

    GameObject myCanvas;

    private Queue<string> sentences; // a queue is like a list but more restrictive, it's also referred to as a FIFO (First in, first out)

   

    private void Update()
    {
        if(sentences.Count == 0 && Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine(EndingDialogue());
        }


        /*if (Input.GetKeyDown(KeyCode.K))
        {
            State dialogue = FindObjectOfType<DialogueScroll>().startingDialogue;
            SkipSentence(dialogue);
        }*/

        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log("Skip is true");
            skip = true;
        }



    }

    private void Start()
    {
        sentences = new Queue<string>();
        myCanvas = GameObject.Find("Canvas");
        myCanvas.SetActive(false);

    }

    public void StartDialogue(State dialogue)
    {
        Debug.Log("Starting conversation " + dialogue.name);

        myCanvas.SetActive(true);

        nameText.text = dialogue.name;

        sentences.Clear(); // Clears sentences from previous conversation

        foreach(string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
            Debug.Log(dialogue.sentences);
            Debug.Log("Sentences after Enqueue " + sentences.Count);
        }

        DisplayNextSentence();

    }


    public void DisplayNextSentence()
    {

        Debug.Log("Sentence count " + sentences.Count);
        if (sentences.Count == 0)
        {
            
            Debug.Log("Stop talking");

            //StartCoroutine(EndingDialogue());
            
            return;
        }

        if (sentences.Count > 0)
        {
            Debug.Log("continue talking");
            string sentence = sentences.Dequeue();
            Debug.Log(sentence);

            //SkipSentence(sentence);
            
            StartCoroutine(TypeSentence(sentence));
        }
        

    }



    /*public void SkipSentence(string sentence)
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log("Fast forward");

            StopAllCoroutines();
            dialogueText.text = "";


            dialogueText.text = sentence;
        }
    }*/

    IEnumerator TypeSentence (string sentence)
    {
        Debug.Log("start coroutine");
        dialogueText.text = ""; //start sentence blank

        
        
        foreach(char letter in sentence.ToCharArray()) // converts our sentence array into characters
        {
            Debug.Log("Typing");
            dialogueText.text += letter;

            if (skip == true)
            {
                dialogueText.text = sentence;
                skip = false;
                StopAllCoroutines();
                yield return null;
            }

            yield return new WaitForSeconds(0.05f);

            
            
        }
        
       

        
        Debug.Log("Waiting for seconds");
    }

    IEnumerator EndingDialogue()
    {

        
            Debug.Log("End dialogue");
            yield return new WaitForSeconds(0);
            StopAllCoroutines();
            sentences.Clear();
            dialogueText.text = "";

            myCanvas.SetActive(false);
        

        

    }


}























    /*[SerializeField] TMP_Text textComponent;
    [SerializeField] State Startingstate;
    bool isScrolling = true;
    State state;

    // Start is called before the first frame update
    void Start()
    {
        state = Startingstate;
        textComponent.text = state.GetDialogue();

        StartCoroutine(DialogueScroll());

        
    }

    private void Update()
    {
        
        ManageState();
        

    }

    IEnumerator DialogueScroll()
    {
        textComponent.ForceMeshUpdate();

        int totalVisibleCharacters = textComponent.textInfo.characterCount; //Get # of visible characters
       
        int counter = 0;
        

        while (isScrolling == true)
        {

            int visibleCount = counter % (totalVisibleCharacters + 1);
            textComponent.maxVisibleCharacters = visibleCount;


            if (visibleCount <= totalVisibleCharacters)
            {
                counter += 1;
                yield return new WaitForSeconds(1.0f);
                Debug.Log("Increase counter");
                Debug.Log("counter " + counter);
            }

            
            

           
            else yield return null;
            
        }
    }

    private void ManageState()
    {
        textComponent.ForceMeshUpdate();
        var newState = state.GetNextState();

        for (int index = 0; index < newState.Length; index++)
        {
            if (Input.GetMouseButtonDown(1))
            {
                isScrolling = false;
                state = newState[index];
                Debug.Log("change text");
                textComponent.text = state.GetDialogue();
                StartCoroutine(DialogueScroll());
            }
            
        }

        

    }*/
    
    

