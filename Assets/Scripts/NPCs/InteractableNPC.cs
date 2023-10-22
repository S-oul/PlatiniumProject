using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableNPC : NPC, IChattyNPC, ITaskNPC
{

    [SerializeField] List<string> _dialogues = new List<string>();
    public List<string> dialogueTexts { get => _dialogues; set =>  _dialogues = value; }

    [SerializeField] GameObject _npcTask;

    [SerializeField] DataManager.TaskEnum _typeTask;
    public GameObject task { get => _npcTask; }

    
    private void Awake()
    {
        _npcTask = Instantiate(DataManager.Instance.AllTasks[(int)_typeTask], this.transform);
        
        //_npcTask = DataManager.Instance.AllTasks[(int)_typeTask];
    }

    public override void Interact(GameObject player)
    {
        _npcTask.GetComponent<Task>().OnPlayerJoinedTask(player);
    }
    public void Talk(string text)
    {
        string currentDialogue = dialogueTexts[Random.Range(0, dialogueTexts.Count)];
        Debug.Log(currentDialogue);
    }
}
