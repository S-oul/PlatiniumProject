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

    bool _hasStarted = false;

    [SerializeField] Transform _goatSpawn;
    [SerializeField] Transform _goatGoal;
    [SerializeField] Transform _playerPos;
    [SerializeField] NPC _monster; 
    DataManager _dataManager;
    PlayerUI _playerUI;
    PlayerController _controller;
    List<PlayerController> _controllers = new List<PlayerController>();
    Animator theAnimator;


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




        _playerUI = PlayersDoingTask[0].GetComponent<PlayerUI>();
        _playerUI.DisplayMashDownButton(true);
        _dataManager = DataManager.Instance;
        foreach (GameObject p in PlayersDoingTask)
        {
            _controller = p.GetComponent<PlayerController>();
            _controllers.Add(_controller);
            _controller.DisableMovementEnableInputs();
        }
        PlayersDoingTask[0].transform.position = _playerPos.position;
        theAnimator = GetComponentInChildren<Animator>();
    }

    public override void End(bool IsSuccess)
    {
        theAnimator.SetTrigger("GoatIdle");
        if (IsSuccess)
        {
            AudioManager.instance.PlaySFXOS("UndergroundCreatureEat", _monster.gameObject.GetComponent<AudioSource>());
            print("GG : Remaining " + _actualTime);    
        }
        else
        {
            print("Noob : Was at " + _goatPos);

        }
        _playerUI.DisplayMashDownButton(false);
        foreach (PlayerController controller in _controllers)
        {
            controller.EnableMovementDisableInputs();
        }
        base.End(IsSuccess);
    }

    private void Update()
    {
        if(!_hasStarted && IsStarted)
        {
            _hasStarted = true;
            theAnimator.SetTrigger("GoatHold");
        }
        
        if (IsStarted && !IsDone)
        {
            _actualTime -= Time.deltaTime;
            if (_actualTime < 0)
            {
                End(false);
            }
            bool hasclickedOnce = false;
            foreach(PlayerController controller in _controllers)
            {
                if (controller.currentContextName != "" && _dataManager.InputNamesConverter[controller.currentContextName] == InputsToString[_buttonToPress])
                {
                    hasclickedOnce = true;
                    _goatPos += _playerForce * Time.fixedDeltaTime;
                    controller.currentContextName = "";
                    if (_goatPos >= 1)
                    {
                        End(true);
                    }
                }
            }

            if(!hasclickedOnce)
            {
                _goatPos -= _goatForce * Time.fixedDeltaTime;
            }
            _goatPos = Mathf.Clamp01(_goatPos);
            transform.position = Vector3.Lerp(_goatSpawn.position, _goatGoal.position, _goatPos);

            foreach(GameObject p in PlayersDoingTask)
            {
                p.transform.position = _playerPos.position;
            }
        }
    }
}

