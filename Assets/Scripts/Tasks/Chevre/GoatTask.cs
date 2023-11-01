using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GoatTask : InputTask, ITimedTask
{
    [SerializeField] float _timeToDoTask = 15;
    float _actualTime;

    [SerializeField] float _goatForce = 1;
    [SerializeField] float _playerForce = 1;
    
    [SerializeField] float _goatPos = 1;


    [SerializeField] Transform _goatSpawn;
    [SerializeField] Transform _goatGoal;
    [SerializeField] Transform _playerPos;

    DataManager _dataManager;
    PlayerUI _playerUI;
    PlayerController _controller;


    [SerializeField] Inputs _buttonToPress;

    public float _givenTime => _timeToDoTask;

    public override void StartTask()
    {

        _actualTime = _timeToDoTask - Difficulty * 1.5f;
        
        if(Difficulty == 4) 
        { 
            _actualTime += 1;
            _goatForce *= ((float)(Difficulty) / 2);
        }
        else if (Difficulty == 5) 
        {
            _actualTime += 2;
            _goatForce *= ((float)(Difficulty - 1) / 2);
        }
        else
        {
            _goatForce *= ((float)(Difficulty) / 2);
        }




        _playerUI = PlayerGameObject.GetComponent<PlayerUI>();
        _playerUI.DisplayMashDownButton(true);
        _dataManager = DataManager.Instance;
        _controller = PlayerGameObject.GetComponent<PlayerController>();
        _controller.DisableMovementEnableInputs();
        _playersDoingTask[0].transform.position = _playerPos.position;
    }

    public override void End(bool IsSuccess)
    {
        if (IsSuccess)
        {

            print("GG : Remaining " + _actualTime);    
        }
        else
        {
            print("Noob : Was at " + _goatPos);

        }
        _playerUI.DisplayMashDownButton(false);
        _controller.EnableMovementDisableInputs();
        base.End(IsSuccess);
    }

    private void Update()
    {


        if (IsStarted && !IsDone)
        {
            _actualTime -= Time.deltaTime;
            if (_actualTime < 0)
            {
                End(false);
            }
            if (_controller.currentContextName != "" && _dataManager.InputNamesConverter[_controller.currentContextName] == InputsToString[_buttonToPress])
            {
                _goatPos += _playerForce * Time.fixedDeltaTime;
                _controller.currentContextName = "";
                if(_goatPos >= 1)
                {
                    End(true);
                }
            }
            else
            {
                _goatPos -= _goatForce * Time.fixedDeltaTime;
            }
            _goatPos = Mathf.Clamp01(_goatPos);
            transform.position = Vector3.Lerp(_goatSpawn.position, _goatGoal.position, _goatPos);
            PlayerGameObject.transform.position = _playerPos.position;
        }
    }
}

