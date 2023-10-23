using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RH : NPC, IChattyNPC
{
    [SerializeField] List<string> _dialogues = new List<string>();
    public List<string> dialogueTexts { get => _dialogues; set => _dialogues = value; }
    

    public void DisplayPlayer(GameObject player)
    {

        NPCUI.ChangeBubbleContent(player.transform.GetComponent<SpriteRenderer>().sprite);
    }

    public void Talk(string text)
    {
        NPCUI.ChangeBubbleContent(text);
    }

}
