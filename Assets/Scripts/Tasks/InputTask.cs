using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.InputSystem;
using System;
using static CowboyQTE;

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
    public enum Inputs
    {
        X,
        Y,
        A,
        B,
        L1,
        L2,
/*        L3,*/
        R1,
        R2,
/*        R3*/
    }
    Dictionary<Inputs, string> _inputsToString = new Dictionary<Inputs, string>()
    {
        {Inputs.X, "X" },
        {Inputs.Y, "Y" },
        {Inputs.A, "A"},
        {Inputs.B, "B"},
        {Inputs.R1, "R1" },
        {Inputs.R2, "R2" },
/*        {Inputs.R3, "R3" },*/
        {Inputs.L1, "L1"},
        {Inputs.L2, "L2"},
/*        {Inputs.L3, "L3" }*/
    };

    protected enum QTEAccuracy
    {
        Missed,
        Ok,
        Great,
        Perfect
    }

    public Dictionary<Inputs, string> InputsToString { get => _inputsToString; set => _inputsToString = value; }
    public abstract void StartTask();
    
    public override void Init()
    {

        IsStarted = true;
        base.Init();
        
        _data = DataManager.Instance;
        StartTask();
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
            print("Right: " + contextName + " // " + inputNeeded);
            contextName = "";
            _inputValue = PlayerInputValue.RightValue;
            
            return PlayerInputValue.RightValue;
        }
        else
        {
            print("Wrong: " + contextName + " // " + inputNeeded);
            _inputValue = PlayerInputValue.WrongValue;
            return PlayerInputValue.WrongValue;
        }
    }

    

}
