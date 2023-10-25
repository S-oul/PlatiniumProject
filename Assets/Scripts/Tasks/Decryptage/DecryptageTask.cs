using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DecryptageTask : InputTask
{
    
    [SerializeField] ObstacleManager _obstacles;
    [SerializeField] PlayerPointerMover _arrow;
    [SerializeField] PlayerInput _plyrInput;

    public PlayerInput PlyrInput { get => _plyrInput; set => _plyrInput = value; }

    public override void End(bool isSuccessful)
    {
        if (!isSuccessful)
        {
        }
        _plyrInput.transform.GetComponent<PlayerController>().EnableMovementDisableInputs();
        _obstacles.DoSpin = false;
    }
    public override void Init()
    {
        for(int i = 0; i < _obstacles.speedList.Count; i++ )
        {
            _obstacles.speedList[i] = _obstacles.speedList[i] * _difficulty;
        }
        StartTask();
    }

    private void Update()
    {
        if (IsStarted)
        {
            _arrow.transform.position += new Vector3(Input.GetAxis("Horizontal"), 0, 0) * Time.deltaTime;
        }
    }

    public override void StartTask()
    {
        //print(_playerInput);
        IsStarted = true;
        _obstacles.DoSpin = true;
    }
}