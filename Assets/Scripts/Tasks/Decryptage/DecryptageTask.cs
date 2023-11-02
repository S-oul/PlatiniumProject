using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DecryptageTask : Task
{
    
    [SerializeField] ObstacleManager _obstacles;
    [SerializeField] PlayerPointerMover _arrow;
    PlayerController _controller;


    public override void End(bool isSuccessful)
    {
        if (!isSuccessful)
        {
        }
        _controller.DisableDecryptageEnableMovements();
        _obstacles.DoSpin = false;
        base.End(isSuccessful);
    }
    public override void Init()
    {
        base.Init();
        _controller = PlayerGameObject.GetComponent<PlayerController>();
        for(int i = 0; i < _obstacles.speedList.Count; i++ )
        {
            _obstacles.speedList[i] = _obstacles.speedList[i] * Difficulty;
        }
        _controller.EnableDecryptageDisableMovements();
        StartTask();
    }

    private void Update()
    {
        if (IsStarted)
        {
            switch (_controller.DecrytContext.x)
            {
                case > 0:
                    print(_controller.DecrytContext);
                    _arrow.MovePlayerForward();
                    _controller.DecrytContext = Vector2.zero;
                    break;
                case < 0:
                    print(_controller.DecrytContext);
                    _arrow.MovePlayerBack();
                    _controller.DecrytContext = Vector2.zero;
                    break;
                default:
                    _controller.DecrytContext = Vector2.zero;
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