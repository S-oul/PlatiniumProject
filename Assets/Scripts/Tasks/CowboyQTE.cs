using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CowboyQTE : InputTask
{
    #region Variables
    protected PlayerInput _playerInput;
    protected PlayerUI _playerUI;

    protected PlayerController _controller;

    [Header("QTE variables")]
    [SerializeField] List<Inputs> _inputsNeeded;
    [SerializeField] int _numberOfInputs = 1;

    

    Inputs _currentInput;

    string _contextName;
    int _currentInputID = 0;
    Coroutine _inputCoroutine;

    public float _timeToDoQTE = 3f;
    int _numberOfFails = 0;

    CowboyNPC _npcCowboy;

    [SerializeField] float _repulseForce = 100f;
    #endregion

    private void Start()
    {
        
        _npcCowboy = transform.parent.parent.GetComponentInChildren<CowboyNPC>();
    }

    public override void StartTask()
    {
        _playerInput = PlayerGameObject.GetComponent<PlayerInput>();
        _playerUI = PlayerGameObject.GetComponent<PlayerUI>();
        _controller = PlayerGameObject.GetComponent<PlayerController>();
        _controller.DisableMovementEnableInputs();
        _playerUI.DisplayQTEUI(true);
        _playerUI.DisplayCowboyQTEUI(true);
        _playerUI.ChangeUIInputsValidation(1);
        _numberOfFails = 0;
        StartTaskQTE();

        _npcCowboy.GetComponentInChildren<Animator>().SetTrigger("GameStart");
    }

    void StartTaskQTE()
    {
        StopAllCoroutines();
        _inputsNeeded.Clear();
        _contextName = "";
        _currentInputID = 0;
        for (int i = 0; i < _numberOfInputs; i++)
        {
            Inputs newInput = (Inputs)((int)(Random.Range(0, 10)));
            _inputsNeeded.Add(newInput);
        }
        
        DisplayInput(_inputsNeeded[0]);
    }

    //Timer to press the input
    IEnumerator TimerToPressInput(float time)
    {
        float _tempTime = time;
        while (CheckInputValue(_controller.currentContextName, InputsToString[_currentInput], _controller) == PlayerInputValue.None && time > 0)
        {
            time -= Time.deltaTime;
            _playerUI.SliderPercentValue = Mathf.InverseLerp(0, _tempTime, time);
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

    }



    //Display a new input
    void DisplayInput(Inputs input)
    {
        if(_inputCoroutine != null)
        {
            StopCoroutine(_inputCoroutine);
        }
        _playerUI.SliderPercentValue = 1f;
        _playerUI.ChangeUIInputs(Color.white);
        _playerUI.ChangeUIInputs(InputsToString[input]);
        _currentInput = input;
        _inputCoroutine = StartCoroutine(TimerToPressInput(_timeToDoQTE));
        _npcCowboy.Fire();
    }


    //Action when the input is the wrong or the right one
    void InputValue(bool isInputRight)
    {
        _controller.currentContextName = "";
        if (isInputRight)
        {
            _currentInputID++;
            if (_currentInputID ==_numberOfInputs)
            {
                //_playerUI.ChangeUIInputsValidation(_index, Color.green);
                EndQTE(true);
                return;
            }
            //Stack overflow because it goes here directly
            else
            {
                //Display Input fait une overflow
                // _playerUI.ChangeUIInputsValidation(_index, Color.green);
                
                DisplayInput(_inputsNeeded[_currentInputID]);
                return;
            }
        }
        else
        {
            _numberOfFails++;
            FeedBackBadInputs();
            if (_numberOfFails == 3)
            {
                // _playerUI.ChangeUIInputsValidation(_index, Color.red);
                EndQTE(false);
                PushBack();
                return;
            }
            else
            {
                //_playerUI.ChangeUIInputsValidation(_index, Color.red);

                //Start Task fait une overflow
                StartTaskQTE();
                return;
            }
            
        }

    }

    void FeedBackBadInputs()
    {
        float value = 1f - ((float)_numberOfFails / (float)_numberOfInputs);
        _playerUI.ChangeUIInputsValidation(value);
    }

    void PushBack()
    {
        Vector2 _dir = new Vector2(-1 * (_npcCowboy.gameObject.transform.position.x - PlayerGameObject.transform.position.x), 0).normalized;
        PlayerGameObject.GetComponent<Rigidbody2D>().AddForce(_dir * _repulseForce, ForceMode2D.Impulse);
    }

    void EndQTE(bool value)
    {
        _controller.EnableMovementDisableInputs();
        _playerInput.actions["InputTask"].Disable();
        _playerUI.ClearUIInputs();
        _playerUI.DisplayInputsUI(false);
        _playerUI.DisplayCowboyQTEUI(true);
        End(value);
        _npcCowboy.GetComponentInChildren<Animator>().SetTrigger("GameEnd");
    }
}
