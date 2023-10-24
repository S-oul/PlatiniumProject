using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeskRH : Object
{
    GameObject _player;
    bool _isUsed;

    public GameObject Player { get =>  _player; }

    public bool IsUsed { get => _isUsed; set => _isUsed = value; }
    GameObject _task;
    [SerializeField] DataManager.TaskEnum _typeTask;

    public GameObject Task { get => _task; }

    private void Awake()
    {
        _task = Instantiate(DataManager.Instance.AllTasks[(int)_typeTask], this.transform);
    }
    public override void Interact(GameObject player)
    {
        if(!_isUsed)
        {
            _player = player;
            _isUsed = true;
            _task.GetComponent<Task>().Init();
        }
        
        

    }


}
