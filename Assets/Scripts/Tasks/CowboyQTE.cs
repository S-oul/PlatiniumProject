using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.InputSystem;

public class CowboyQTE : InputTask
{
    #region Variables
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
        _numberOfFails = 0;
        StartTaskQTE();
    }

    void StartTaskQTE()
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
                End(true);
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
                End(false);
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
        Vector2 _dir = new Vector2(-1 * (_npcCowboy.gameObject.transform.position.x - _player.transform.position.x), 0).normalized;
        _player.GetComponent<Rigidbody2D>().AddForce(_dir * _repulseForce, ForceMode2D.Impulse);
    }
}
