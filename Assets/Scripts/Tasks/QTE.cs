using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.InputSystem;

public class QTE : InputTask, ITimedTask
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

    QTEInputs _currentInput;
    bool _isQTETimeUp = false;
    bool _isInputTimeUp = false;
    bool _hasPressedRightInput = false;

    string _contextName;
    int _currentInputID = 0;
    Coroutine _inputCoroutine;


    

    public override void StartTask()
    {
        StopAllCoroutines();
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
        
        while (CheckInputValue(_contextName, _dicInputs[_currentInput]) == PlayerInputValue.None && time > 0)
        {
            time -= Time.deltaTime;
            _playerUI._sliderPercentValue = Mathf.InverseLerp(0, _tempTime, time);
            yield return null;

        }
        if (time <= 0)
        {
            _isInputTimeUp = true;
            InputValue(false);
            yield return null;
        }

        else if (_inputValue == PlayerInputValue.WrongValue)
        {
            InputValue(false);
            yield return null;

        }
        else if (_inputValue == PlayerInputValue.RightValue)
        {
            InputValue(true);
            yield return null;
        }



        //OnTaskCompleted?.Invoke(this);
    }


    //=> Global timer for the task
    IEnumerator TimeToDoQTE()
    {
        yield return new WaitForSeconds(_givenTime);
        _isQTETimeUp = true;
        StartTask();
    }


    //Display a new input
    void DisplayInput(QTEInputs input)
    {
        _contextName = "";
        _isInputTimeUp = false;
        _playerUI._sliderPercentValue = 1f;
        _playerUI.ChangeUIInputs(Color.white);
        _playerUI.ChangeUIInputs(_dicInputs[input]);
        _currentInput = input;
        _inputCoroutine = StartCoroutine(TimerToPressInput(_data.TimeBetweenInputsQTE[_difficulty - 1], input));
    }


    //Action when the input is the wrong or the right one
    void InputValue(bool isInputRight)
    {
        Debug.Log("Input right: " + isInputRight);
        StopCoroutine(_inputCoroutine);
        _contextName = "";
        if (isInputRight)
        {
            _playerUI.ChangeUIInputs(Color.green);
            _currentInputID++;
            if (_currentInputID == _inputsNeeded.Count)
            {
                Debug.Log("Finito");
                return;
            }
            //Stack overflow because it goes here directly
            else if (_currentInputID != _inputsNeeded.Count)
            {
                Debug.Log("Pas finito");
                //Display Input fait une overflow
                DisplayInput(_inputsNeeded[_currentInputID]);
                return;
            }
        }
        else
        {
            _playerUI.ChangeUIInputs(Color.red);
            //Start Task fait une overflow
            //StartTask();
            return;
        }
    }



}
