using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TamponageTask : InputTask, ITimedTask
{
    
    // Start is called before the first frame update

    [SerializeField] int _numOfClicks;
    [SerializeField] float _timeLimit;

    [SerializeField] Inputs _inputToPress;
    string _inputName;

    string _inputPressedP1;
    string _inputPressedP2;

    PlayerController _player1;
    PlayerController _player2;

    int _remainingClicks;
    float _remainingTime;

    string _lastPressed = "";

    PlayerController _lastPlayer;

    bool _inputsCanBePressed = false;

    [SerializeField] float _angle = 360;

    public float _givenTime { get => _timeLimit; set => _timeLimit = value; }

    Transform _clock;

    bool _canCheckInput;
    private void Start()
    {
        _inputName = InputsToString[_inputToPress];
        _clock = gameObject.transform.parent.parent.Find("Timer").GetChild(0).GetChild(0);
        _canCheckInput = false;
    }
    public override void End(bool isSuccessful)
    {
        
    }


    public override void StartTask()
    {
        _angle = 360f;
        _canCheckInput = true;
        _inputsCanBePressed = true;
        _player1 = PlayersDoingTask[0].GetComponent<PlayerController>();
        _player2 = PlayersDoingTask[1].GetComponent<PlayerController>();
        _lastPlayer = _player2;
        _remainingTime = _timeLimit;
        StartCoroutine(TimerTask());
        GameLoop();
        
    }

    void GameLoop()
    {
        _inputsCanBePressed = true;
    }

    void ResultInput()
    {
        if (_canCheckInput)
        {
            
            _canCheckInput = false;
            if (CheckInputValue(_player1.currentContextName, _inputName, _player1) == PlayerInputValue.RightValue)
            {
                _inputPressedP1 = _player1.currentContextName;


            }
            else
            {

                _inputPressedP1 = "";
            }
            if (CheckInputValue(_player2.currentContextName, _inputName, _player2) == PlayerInputValue.RightValue)
            {

                _inputPressedP2 = _player2.currentContextName;

            }
            else
            {
                _inputPressedP2 = "";
            }
            ValueAction();
        }
        
        
       
        
        
       
    }

    IEnumerator TimerTask()
    {
        float timePercent;
        while( _remainingTime > 0)
        {
            ResultInput();
            _remainingTime -= Time.deltaTime;
            timePercent = Mathf.InverseLerp(0, _timeLimit, _remainingTime);
            _clock.eulerAngles = new Vector3(0, 0, _angle * timePercent);
            yield return null;
        }
        
    }

    void ValueAction()
    {
        if (_inputPressedP1 == "" && _inputPressedP2 != "" )
        {
            if(_lastPlayer == _player1)
            {
                Debug.Log("Couber");
                _lastPlayer = _player2;
            }
            else if(_lastPlayer == _player2)
            {
                StartCoroutine(Penality());
                return;
            }
            
        }
        if(_inputPressedP2 == "" && _inputPressedP1 != "")
        {
            if (_lastPlayer == _player2)
            {
                Debug.Log("Couber");
                _lastPlayer = _player1;
            }
            else if (_lastPlayer == _player1)
            {
                StartCoroutine(Penality());
                return;
            }
        }
        _inputPressedP1 = "";
        _inputPressedP2 = "";
        _canCheckInput = true;
    }
     IEnumerator Penality()
    {
        _inputPressedP1 = "";
        _inputPressedP2 = "";
        Debug.Log("Penality");
        _player1.GetComponent<PlayerController>().DisableAllInputs();
        _player2.GetComponent<PlayerController>().DisableAllInputs();
        yield return new WaitForSeconds(5);
        _player1.GetComponent<PlayerController>().DisableMovementEnableInputs();
        _player2.GetComponent<PlayerController>().DisableMovementEnableInputs();
        _canCheckInput = true;

    }



}
