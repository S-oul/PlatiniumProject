using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.InputSystem;
using System;

public abstract class InputTask : Task
{
    protected PlayerInput _playerInput;
    protected PlayerUI _playerUI;
    protected DataManager _data;
    protected PlayerController _controller;

    string _name;

    bool _InputHasBeenPressed = false;

    protected PlayerInputValue _inputValue;

    public enum PlayerInputValue
    {
        None,
        RightValue,
        WrongValue
    }
    
    public abstract void StartTask();
    
    public override void Init()
    {
        
        _playerInput = _player.GetComponent<PlayerInput>();
        _playerUI = _player.GetComponent<PlayerUI>();
        _controller = _player.GetComponent<PlayerController>();
        _data = DataManager.Instance;
        _controller.DisableMovementExceptInteract();
        _playerUI.DisplayUI(true);
        _playerUI.ChangeUIInputsValidation(1);
        StartTask();
    }

    public override void End(bool isSuccessful)
    {
        
        IsDone = isSuccessful;
        _controller.EnableMovement();
        _playerInput.actions["InputTask"].Disable();
        _playerUI.ClearUIInputs();
        _playerUI.DisplayUI(false);

    }
    public PlayerInputValue CheckInputValue(string contextName, string inputNeeded)
    {
        contextName = _controller.currentContextName;
        if (string.IsNullOrEmpty(contextName))
        {
            _inputValue = PlayerInputValue.None;
            return PlayerInputValue.None;
        }
        
        _data.InputNamesConverter.TryGetValue(contextName, out string userInputName);
        if (userInputName == inputNeeded)
        {
            contextName = "";
            _inputValue = PlayerInputValue.RightValue;
            
            return PlayerInputValue.RightValue;
        }
        else
        {
            _inputValue = PlayerInputValue.WrongValue;
            return PlayerInputValue.WrongValue;
        }
    }

    

}
