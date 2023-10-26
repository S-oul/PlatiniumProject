using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RH : NPC, IChattyNPC
{
    [SerializeField] List<string> _dialogues = new List<string>();
    public List<string> dialogueTexts { get => _dialogues; set => _dialogues = value; }
    Task _task;
    bool _isPlayerNeeded = false;
    GameObject _playerNeeded;

    public bool IsPlayerNeeded { get => _isPlayerNeeded; set => _isPlayerNeeded = value; } 
    public Task TaskRH { get => _task; set => _task = value; }
    public GameObject PlayerNeeded { get => _playerNeeded; set => _playerNeeded = value; }

    private void Start()
    {
        _isPlayerNeeded = false;
    }

    public void DisplayPlayer(GameObject player)
    {
        NPCUI.DisplayTalkingBubble(true);
        if(player == null)
        {
            NPCUI.ChangeBubbleImage(null);
        }
        else
        {
            NPCUI.ChangeBubbleImage(player.transform.GetComponent<SpriteRenderer>().sprite);
        }
        
    }

    public void Talk(string text)
    {
        
        NPCUI.DisplayTalkingBubble(true);
        NPCUI.ChangeBubbleText(text);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(_isPlayerNeeded && collision.gameObject == _playerNeeded)
        {
            _task.End(true);
        }
        else if(_isPlayerNeeded && collision.tag == "Player")
        {
            _task.End(false);
        }
    }
}
