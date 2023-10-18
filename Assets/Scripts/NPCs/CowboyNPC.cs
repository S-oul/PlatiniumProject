using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowboyNPC : NPC, IChattyNPC, ITaskNPC
{

    [SerializeField] List<string> _dialogues = new List<string>();
    public List<string> dialogueTexts { get => _dialogues; set =>  _dialogues = value; }

    [SerializeField] Task _npcTask;

    [SerializeField] DataManager.TaskEnum _typeTask;
    public Task task { get => _npcTask; }

    
    private void Awake()
    {
        _npcTask = DataManager.Instance.AllTasks[(int)_typeTask];
    }

    public override void Interact()
    {
        string currentDialogue = dialogueTexts[Random.Range(0, dialogueTexts.Count)];
        Talk(currentDialogue);
    }
    public void Talk(string text)
    {
        Debug.Log(_name + ": " + text);
    }
}
