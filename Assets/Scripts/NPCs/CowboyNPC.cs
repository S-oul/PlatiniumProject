using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowboyNPC : NPC, IChattyNPC
{

    [SerializeField] List<string> _dialogues = new List<string>();
    public List<string> dialogueTexts { get => _dialogues; set =>  _dialogues = value; }
    public void Talk(string text)
    {
        string currentDialogue = dialogueTexts[Random.Range(0, dialogueTexts.Count)];
        Debug.Log(currentDialogue);
    }

    public override void Interact(GameObject player)
    {
        return;
    }
}
