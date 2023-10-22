using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.InputSystem;

public class CowboyQTE : InputTask, ITimedTask
{
    #region Variables
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
    [SerializeField] int _numberOfInputs = 1;

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

    QTEInputs _currentInput;

    string _contextName;
    int _currentInputID = 0;
    Coroutine _inputCoroutine;

    public float _timeToDoQTE = 3f;

    bool _isCurrentInputRight = false;
    bool _wasLastInputRight = false;
    int _index = 0;
    int _numberOfFails = 0;
    bool _isFirstTimeGuessing = true;
    #endregion

    public override void StartTask()
    {
        StopAllCoroutines();
        _inputsNeeded.Clear();
        _contextName = "";
        _currentInputID = 0;
        for (int i = 0; i < _numberOfInputs; i++)
        {
            QTEInputs newInput = (QTEInputs)((int)(Random.Range(0, 9)));
            _inputsNeeded.Add(newInput);
        }
        StartCoroutine(TimeToDoQTE());
        DisplayInput(_inputsNeeded[0]);
    }

    //Timer to press the input
    IEnumerator TimerToPressInput(float time)
    {
        float _tempTime = time;
        while (CheckInputValue(_contextName, _dicInputs[_currentInput]) == PlayerInputValue.None && time > 0)
        {
            time -= Time.deltaTime;
            _playerUI._sliderPercentValue = Mathf.InverseLerp(0, _tempTime, time);
            yield return null; //=> Inportant => Inbecile

        }
        if (time <= 0)
        {
            InputValue(false);
        }

        else if (_inputValue == PlayerInputValue.WrongValue)
        {
            InputValue(false);

        }
        else if (_inputValue == PlayerInputValue.RightValue)
        {
            InputValue(true);

        }



        OnTaskCompleted?.Invoke(this);
    }


    //=> Global timer for the task
    IEnumerator TimeToDoQTE()
    {
        yield return new WaitForSeconds(_givenTime);
        StartTask();
    }


    //Display a new input
    void DisplayInput(QTEInputs input)
    {
        if(_inputCoroutine != null)
        {
            StopCoroutine(_inputCoroutine);
        }
        _playerUI._sliderPercentValue = 1f;
        _playerUI.ChangeUIInputs(Color.white);
        _playerUI.ChangeUIInputs(_dicInputs[input]);
        _currentInput = input;
        _inputCoroutine = StartCoroutine(TimerToPressInput(_timeToDoQTE));
    }


    //Action when the input is the wrong or the right one
    void InputValue(bool isInputRight)
    {
        _isCurrentInputRight = isInputRight;
        _controller.currentContextName = "";
        if (isInputRight)
        {
            _currentInputID++;
            if (_currentInputID == _inputsNeeded.Count)
            {
                Debug.Log("Win");
                //_playerUI.ChangeUIInputsValidation(_index, Color.green);
                End(true);
                return;
            }
            //Stack overflow because it goes here directly
            else
            {
                //Display Input fait une overflow
               // _playerUI.ChangeUIInputsValidation(_index, Color.green);
                IndexValue(true);
                _wasLastInputRight = _isCurrentInputRight;
                DisplayInput(_inputsNeeded[_currentInputID]);
                return;
            }
        }
        else
        {
            _numberOfFails++;
            if (_numberOfFails == 3)
            {
                Debug.Log("Lose");
               // _playerUI.ChangeUIInputsValidation(_index, Color.red);
                End(false);
                return;
            }
            else
            {
                //_playerUI.ChangeUIInputsValidation(_index, Color.red);
                IndexValue(false);
                _wasLastInputRight = _isCurrentInputRight;
                //Start Task fait une overflow
                StartTask();
                return;
            }
            
        }

    }

    void IndexValue(bool rightInput)
    {
        
        Debug.Log("Last Input = " + _wasLastInputRight + " | Current Input = " + _isCurrentInputRight);
        Debug.Log(_index);
        if (_wasLastInputRight != _isCurrentInputRight)
        {
            if (_isFirstTimeGuessing)
            {
                _isFirstTimeGuessing = false;
            }
            else
            {
                Debug.Log("Clear");
                _playerUI.ClearUIInputsValidation();
            }
            _index = 0;
        }
        if (rightInput)
        {
            _playerUI.ChangeUIInputsValidation(_index, Color.green);
        }
        else
        {
            _playerUI.ChangeUIInputsValidation(_index, Color.red);
        }
        
        _index++;
    }
}
