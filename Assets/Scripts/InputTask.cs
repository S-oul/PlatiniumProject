using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.InputSystem;
using System;

public abstract class InputTask : Task
{
    protected DataManager _data;


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
        IsStarted = true;
        
        
        _data = DataManager.Instance;
        StartTask();
    }

    public override void End(bool isSuccessful)
    {
        IsStarted = false;
        IsDone = isSuccessful;
        

    }
    public PlayerInputValue CheckInputValue(string contextName, string inputNeeded, PlayerController controller)
    {
        contextName = controller.currentContextName;
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
