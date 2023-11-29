using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeskRH : Object
{
    GameObject _player;

    public GameObject Player { get =>  _player; }

    GameObject _task;
    [SerializeField] DataManager.TaskEnum _typeTask;

    public GameObject Task { get => _task; }

    private void Awake()
    {
        _task = Instantiate(DataManager.Instance.AllTasks[(int)_typeTask], this.transform);
    }
    public override void Interact(GameObject player)
    {
        if(!IsUsed)
        {
            _player = player;
            IsUsed = true;
            _task.GetComponent<Task>().PlayerGameObject = player;
            _task.GetComponent<Task>().Init();
        }
        
        

    }


}
