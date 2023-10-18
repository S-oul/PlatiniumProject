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
        Debug.Log("Input Value = " + _inputValue);
        if (time <= 0)
        {
            InputValue(false);
        }

        else if (_inputValue == PlayerInputValue.WrongValue)
        {
            /*Debug.Log("Faux");*/
            InputValue(false);

        }
        else if (_inputValue == PlayerInputValue.RightValue)
        {
            /*Debug.Log("Vraie");*/
            InputValue(true);

        }



        //OnTaskCompleted?.Invoke(this);
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
        Debug.Log("Display");
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
        _controller.currentContextName = "";
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
            else
            {
                /*Debug.Log("Pas finito");*/
                //Display Input fait une overflow
                DisplayInput(_inputsNeeded[_currentInputID]);
                return;
            }
        }
        else
        {
            _playerUI.ChangeUIInputs(Color.red);
            //Start Task fait une overflow
            StartTask();
            return;
        }
    }



}
