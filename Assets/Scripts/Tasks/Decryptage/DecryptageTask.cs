using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DecryptageTask : Task
{
    
    [SerializeField] ObstacleManager _obstacles;
    [SerializeField] PlayerPointerMover _arrow;
    [SerializeField] PlayerController _controller;

    public PlayerController Controller { get => _controller; set => _controller = value; }

    public override void End(bool isSuccessful)
    {
        if (!isSuccessful)
        {
        }
        _controller.EnableMovementDisableInputs();
        _obstacles.DoSpin = false;
        base.End(isSuccessful);
    }
    public override void Init()
    {
        for(int i = 0; i < _obstacles.speedList.Count; i++ )
        {
            _obstacles.speedList[i] = _obstacles.speedList[i] * Difficulty;
        }
        StartTask();
    }

    private void Update()
    {
        if (IsStarted)
        {
            switch (_controller.DecrytContext)
            {
                case > 0:
                    print(_controller.DecrytContext);
                    _arrow.MovePlayerForward();
                    _controller.DecrytContext = 0;
                    break;
                case < 0:
                    print(_controller.DecrytContext);
                    _arrow.MovePlayerBack();
                    _controller.DecrytContext = 0;
                    break;
                default:
                    _controller.DecrytContext = 0;
                    break;
            }
        }
    }

    public void StartTask()
    {
        IsStarted = true;
        _obstacles.DoSpin = true;
    }
}