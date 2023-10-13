using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.InputSystem;
using System;

public class InputTask : Task
{
    PlayerInput _playerInput;
    PlayerUI _playerUI;
    DataManager _data;
    PlayerController _controller;

    string _name;

    bool _InputHasBeenPressed = false;
    public override void StartTask()
    {
        Debug.Log("Yes");
        _playerInput = _player.GetComponent<PlayerInput>();
        _playerUI = _player.GetComponent<PlayerUI>();
        _playerInput.actions["Interact"].Disable();
        _playerInput.actions["Movement"].Disable();
        _playerInput.actions["Jump"].Disable();
        _playerInput.actions["Crouch"].Disable();
        _playerInput.actions["InputTask"].Enable();
        _controller = _player.GetComponent<PlayerController>();
        _data = DataManager.Instance;
    }



}
