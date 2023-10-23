using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeskRH : Object
{
    GameObject _player;
    bool _isUsed;

    [SerializeField] Task _task;

    public GameObject Player { get =>  _player; }

    public bool IsUsed { get => _isUsed; set => _isUsed = value; }

    public override void Interact(GameObject player)
    {
        if(!_isUsed)
        {
            _player = player;
            _isUsed = true;
            _task.Init();
        }
        
        

    }


}
