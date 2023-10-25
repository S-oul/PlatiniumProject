using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TamponageTask : InputTask, ITimedTask
{
    
    // Start is called before the first frame update

    int _numOfClicksDone = 0;
    [SerializeField] int _numOfClicksToDo = 10;

    [SerializeField] float _timeLimit;

    [SerializeField] Inputs _inputToPress;
    string _inputName;

    int _p1Value = 0;
    int _p2Value = 0;


    PlayerController _player1;
    PlayerController _player2;

    //int _remainingClicks;
    float _remainingTime;


    [SerializeField] float _angle = 360;

    public float _givenTime { get => _timeLimit; set => _timeLimit = value; }

    Transform _clock;

    private void Start()
    {
        _numOfClicksToDo *= Difficulty;
        _inputName = InputsToString[_inputToPress];
        _clock = gameObject.transform.parent.parent.Find("Timer").GetChild(0).GetChild(0);
    }
    public override void End(bool isSuccessful)
    {
        
    }


    public override void StartTask()
    {
        _angle = 360f;
        _player1 = PlayersDoingTask[0].GetComponent<PlayerController>();
        _player2 = PlayersDoingTask[1].GetComponent<PlayerController>();
        _remainingTime = _timeLimit;
        StartCoroutine(TimerTask());
        IsStarted = true;
    }
    
    void Update()
    {
        if (IsStarted) 
        {
            if (_p1Value == 2 || _p2Value == 2)
            {
                StartCoroutine(Penality());
            }
            if (_p2Value ==  1 && _p1Value == 0)
            {
                StartCoroutine(Penality());
            }

            if(_p1Value == 1 && _p2Value == 1)
            {
                _numOfClicksDone++;
                _p1Value = 0;
                _p2Value = 0;
            }
            if (_player1.currentContextName == _inputName)
            {
                _p1Value++;
            }
            if (_player2.currentContextName == _inputName)
            {
                _p2Value++;
            }
        }
    }

    IEnumerator TimerTask()
    {
        float timePercent;
        while( _remainingTime > 0)
        {
            _remainingTime -= Time.deltaTime;
            timePercent = Mathf.InverseLerp(0, _timeLimit, _remainingTime);
            _clock.eulerAngles = new Vector3(0, 0, _angle * timePercent);
            yield return null;
        }
        
    }

     IEnumerator Penality()
    {
        _p1Value = 0;
        _p2Value = 0;
        _player1.GetComponent<PlayerController>().DisableAllInputs();
        _player2.GetComponent<PlayerController>().DisableAllInputs();
        yield return new WaitForSeconds(5);
        _player1.GetComponent<PlayerController>().DisableMovementEnableInputs();
        _player2.GetComponent<PlayerController>().DisableMovementEnableInputs();

    }



}
