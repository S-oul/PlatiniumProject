using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeskDuolingo : Object
{
    Task _task;
    public Task TaskDuolingo { get => _task; set => _task = value; }

    private void Awake()
    {
        //_task = Instantiate(DataManager.Instance.AllTasks[(int)_typeTask], this.transform);
    }
    public override void Interact(GameObject player)
    {
        player.transform.position = gameObject.transform.Find("PlayerPosition").position;
        player.GetComponent<SpriteRenderer>().sortingOrder = 1;
        player.GetComponent<PlayerController>().DisableMovementExceptInput();
        _task.OnPlayerJoinedTask(player);
    }
}
