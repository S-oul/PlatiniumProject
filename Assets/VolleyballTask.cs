using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class VolleyballTask : InputTask
{
    [SerializeField] List<Transform> _positions;
    [SerializeField] float[] _timeToDoInput = new float[3];
    GameObject _playerTask;
    PlayerController _controller;
    PlayerInput _playerInput;
    PlayerUI _playerUI;

    Inputs _inputsNeeded;

    Inputs _currentInput;

    string _contextName;
    Coroutine _inputCoroutine;

    public float _timeToDoQTE = 3f;
    int _numberOfFails = 0;

    

    public override void StartTask()
    {
        foreach(GameObject player in PlayersDoingTask)
        {
            Transform pos = _positions[(int)Random.Range(0, _positions.Count)];
            player.transform.position = pos.position;
            player.GetComponent<PlayerController>().DisableMovementEnableInputs();
            _positions.Remove(pos);
        }
        _playerUI.DisplayVolleyQTEUI(true);
        StartTaskQTE();
    }

    void StartTaskQTE()
    {
        _contextName = "";
        
    }

    void GameLoop()
    {
        List<GameObject> players = new List<GameObject>();
        foreach(GameObject player in PlayersDoingTask)
        {
            players.Add(player);
        }
        for (int i = 0; i < 3; i++)
        {
            _playerTask = players[Random.Range(0, players.Count)];
            NewInput(_playerTask, _timeToDoInput[i]);
        }
    }
    void NewInput(GameObject player, float time)
    {
        _controller = player.GetComponent<PlayerController>();
        _playerInput = player.GetComponent<PlayerInput>();
        _playerUI = player.GetComponent<PlayerUI>();
        _inputsNeeded = (Inputs)((int)(Random.Range(0, 10)));
        _playerUI.DisplayQTEUI(true);
        _playerUI.DisplayVolleyQTEUI(true);
        TimerToPressInput(time);
        DisplayInput(_inputsNeeded);
    }
    //Timer to press the input
    IEnumerator TimerToPressInput(float time)
    {
        float _tempTime = time;
        while (CheckInputValue(_controller.currentContextName, InputsToString[_currentInput], _controller) == PlayerInputValue.None && time > 0)
        {
            time -= Time.deltaTime;
            _playerUI.ChangeRoundTimerValue(Mathf.InverseLerp(0, _tempTime, time));
            
            yield return null; //=> Inportant => Inbecile

        }
        
        if (time < 0)
        {
            InputValue(false, QTEAccuracy.Missed);
        }

        else if (_inputValue == PlayerInputValue.WrongValue)
        {
            InputValue(false, QTEAccuracy.Missed);

        }
        else if (_inputValue == PlayerInputValue.RightValue)
        {
            switch (time)
            {
                case <= 0:
                    InputValue(false, QTEAccuracy.Missed);
                    break;
                case <= 0.2f:
                    InputValue(true, QTEAccuracy.Ok);
                    break;
                case <= 0.8f:
                    InputValue(true, QTEAccuracy.Great);
                    break;
                case > 0.8f:
                    InputValue(true, QTEAccuracy.Perfect);
                    break;
            }

        }

    }



    //Display a new input
    void DisplayInput(Inputs input)
    {
        if (_inputCoroutine != null)
        {
            StopCoroutine(_inputCoroutine);
        }
        _playerUI.SliderPercentValue = 1f;
        _playerUI.ChangeUIInputs(Color.white);
        _playerUI.ChangeUIInputs(InputsToString[input]);
        _currentInput = input;
        _inputCoroutine = StartCoroutine(TimerToPressInput(_timeToDoQTE));
    }


    //Action when the input is the wrong or the right one
    void InputValue(bool isInputRight, QTEAccuracy accuracy)
    {
        _controller.currentContextName = "";
        _playerUI.ResetRoundTimerQTE();
        if (isInputRight)
        {
            switch (accuracy)
            {
                case QTEAccuracy.Ok:
                    Debug.Log("OK");
                    break;
                case QTEAccuracy.Great:
                    Debug.Log("Great");
                    break;
                case QTEAccuracy.Perfect:
                    Debug.Log("Perfect");
                    break;
            }

        }
        else
        {

        }

    }

    void EndQTE(bool value)
    {
        _controller.EnableMovementDisableInputs();
        _playerInput.actions["InputTask"].Disable();
        _playerUI.ClearUIInputs();
        _playerUI.DisplayInputsUI(false);
        _playerUI.DisplayVolleyQTEUI(false);
        End(value);
    }
}
