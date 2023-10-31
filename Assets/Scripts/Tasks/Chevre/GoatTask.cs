using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GoatTask : InputTask
{
    [SerializeField] float _goatForce = 1;
    [SerializeField] float _playerForce = 1;

    [SerializeField] Transform _goatSpawn;
    [SerializeField] Transform _goatGoal;
    [SerializeField] Transform _playerPos;

    DataManager _dataManager;



    [SerializeField] Inputs _buttonToPress;

    PlayerController _controller;
    
    public override void StartTask()
    {
        _dataManager = DataManager.Instance;
        _controller = PlayerGameObject.GetComponent<PlayerController>();
        _controller.DisableMovementEnableInputs();
        _playersDoingTask[0].transform.position = _playerPos.position;

    }

    private void Update()
    {


        if (IsStarted)
        {
            if (_controller.currentContextName != "" && _dataManager.InputNamesConverter[_controller.currentContextName] == InputsToString[_buttonToPress])
            {
                transform.position = Vector3.Lerp(_goatSpawn.position, _goatGoal.position);
                print("heehehhe");
                _controller.currentContextName = "";
            }
            transform.position = Vector3.Lerp(_goatGoal.position, _goatSpawn.position);
        }
    }
}

