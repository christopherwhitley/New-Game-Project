using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueScroll : MonoBehaviour
{
    [SerializeField] public State startingDialogue;
    [SerializeField] public State questDialogue;

    public void TriggerDialogue(State dialogue)
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }
  
  
}
