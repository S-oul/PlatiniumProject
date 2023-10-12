using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.InputSystem;

public class QTE : Task, ITimedTask
{
    public float _givenTime => _givenTimeTask;
    
    [SerializeField] private float _givenTimeTask = 0f;
    InputAction action = new InputAction();
    public enum QTEInputs
    {
        X,
        Y,
        A,
        B,
        L1,
        L2,
        L3,
        R1,
        R2,
        R3
    }


    [Header("QTE variables")]
    [SerializeField] List<QTEInputs> _inputsNeeded;

    Dictionary<QTEInputs, string> _dicInputs = new Dictionary<QTEInputs, string>()
    {
        {QTEInputs.X, "X" },
        {QTEInputs.Y, "Y" },
        {QTEInputs.A, "A"},
        {QTEInputs.B, "B"},
        {QTEInputs.R1, "R1" },
        {QTEInputs.R2, "R2" },
        {QTEInputs.R3, "R3" },
        {QTEInputs.L1, "L1"},
        {QTEInputs.L2, "L2"},
        {QTEInputs.L3, "L3" }
    };

    PlayerController _controller;
    PlayerUI _playerUI;
    PlayerInput _playerInput;
    DataManager _data;
    bool _isQTETimeUp = false;
    bool _isInputTimeUp = false;
    bool _hasPressedRightInput = false;

    string _contextName;

    int _currentInputID = 0;


    public override void StartTask()
    {
        _playerInput = _player.GetComponent<PlayerInput>();
        _playerUI = _player.GetComponent<PlayerUI>();
        _playerInput.actions["Interact"].Disable();
        _playerInput.actions["Movement"].Disable();
        _playerInput.actions["Jump"].Disable();
        _playerInput.actions["Crouch"].Disable();
        _playerInput.actions["QTE"].Enable();
        _controller = _player.GetComponent<PlayerController>();
        action.Enable();
        _data = DataManager.Instance;
        StartQTE();
    }

    void StartQTE()
    {
        _inputsNeeded.Clear();
        _contextName = "";
        _currentInputID = 0;
        for (int i = 0; i < (int)Random.Range(_data.MinQTENumberInputs, _data.MaxQTENumberInputs); i++)
        {
            QTEInputs newInput = (QTEInputs)((int)(Random.Range(0, 9)));
            _inputsNeeded.Add(newInput);
        }
        TimeToDoQTE();
        DisplayInput(_inputsNeeded[0]);
    }

    //Timer to press the input
    IEnumerator TimerToPressInput(float time, QTEInputs input)
    {
        float _tempTime = time;
        while(!CheckIfRightInput(input)  && time > 0)
        {
            
            time -= Time.deltaTime;
            _playerUI._sliderPercentValue = Mathf.InverseLerp(0, _tempTime, time);
            yield return null;

        }
        if(time <= 0)
        {
            _isInputTimeUp = true;
            Debug.Log("Time's up");
            yield return null;
        }
        if (_currentInputID == _inputsNeeded.Count)
        {
            Debug.Log("wp!");
        }
        else if (_currentInputID != _inputsNeeded.Count)
        {
            DisplayInput(_inputsNeeded[_currentInputID]);
        }
    }


    //=> Global timer for the task
    IEnumerator TimeToDoQTE()
    {
        yield return new WaitForSeconds(_givenTime);
        _isQTETimeUp = true;
        StartQTE();
    }


    //Display a new input
    void DisplayInput(QTEInputs input)
    {
        _hasPressedRightInput = false;
        _isInputTimeUp = false;
        _playerUI._sliderPercentValue = 1f;
        _playerUI.ChangeUIInputs(_dicInputs[input]);
        StartCoroutine(TimerToPressInput(_data.TimeBetweenInputsQTE[_difficulty - 1], input));
    }

    bool CheckIfRightInput(QTEInputs input)
    {
        _contextName = _controller.currentContextName;
        if(_contextName != "")
        {
            if (_data.InputNamesConverter[_contextName] == _dicInputs[_inputsNeeded[_currentInputID]])
            {
                _playerUI.ChangeUIInputs(Color.green);
                _hasPressedRightInput = true;
                _contextName = "";
                _currentInputID++;
                
                

                //StopCoroutine(TimerToPressInput(_data.TimeBetweenInputsQTE[_difficulty - 1], input));
                return true;
            }
            else
            {
                //StopCoroutine(TimerToPressInput(_data.TimeBetweenInputsQTE[_difficulty - 1], input));
                //WrongInput();
                return false;
                

            }
        }
        
        else
        {

            return false;
        }
    

     }

    void WrongInput()
    {
        _playerUI.ChangeUIInputs(Color.red);
        _hasPressedRightInput = false;
        _contextName = "";
        StartQTE();
    }

}
