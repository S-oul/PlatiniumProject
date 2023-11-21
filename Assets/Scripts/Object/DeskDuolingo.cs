using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DeskDuolingo : Object
{
    GameObject _task;
    public GameObject TaskDuolingo { get => _task; set => _task = value; }

    private void Start()
    {
        
    }

    public override void Interact(GameObject player)
    {
        player.transform.position = gameObject.transform.Find("PlayerPosition").position;
        player.GetComponent<PlayerController>().BlockPlayer(true);
        player.transform.Find("Animation").GetComponent<SpriteRenderer>().sortingLayerName = "Deco";
        player.GetComponent<PlayerController>().DisableMovementEnableInputs();
        _task.GetComponent<Task>().OnPlayerJoinedTask(player);
    }

    
        
    
}
