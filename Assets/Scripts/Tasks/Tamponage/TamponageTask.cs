using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

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
    PlayerUI _player1UI;
    PlayerUI _player2UI;

    //int _remainingClicks;
    float _remainingTime;


    [SerializeField] float _angle = 360;
    GameObject _activePlayer;
    public float _givenTime { get => _timeLimit; set => _timeLimit = value; }

    Transform _clock;

    TextMeshProUGUI _textScore;

    Coroutine _timer;

    bool _hasTamponnageSoundPlayedP1 = false;
    bool _hasTamponnageSoundPlayedP2 = false;
    private void Start()
    {
        _numOfClicksToDo *= Difficulty;
        _inputName = InputsToString[_inputToPress];
        _clock = gameObject.transform.parent.parent.Find("Timer").GetChild(0).GetChild(0);
        _textScore = gameObject.transform.parent.parent.Find("Score").Find("TextScore").GetComponent<TextMeshProUGUI>();
    }
    public override void End(bool isSuccessful)
    {
        if(isSuccessful == true)
        {
            _textScore.text = "GG!";
            _textScore.color = Color.green;
            IsDone = true;
            
        }
        else
        {
            _textScore.text = "Too bad!";
            _textScore.color = Color.red;
           
        }
        StopCoroutine(_timer);
        foreach(GameObject player in PlayersDoingTask)
        {
            
            player.transform.position = gameObject.transform.parent.parent.Find("PlayerRespawnPoint").position;
            player.GetComponent<PlayerController>().BlockPlayer(false);
            player.GetComponent<SpriteRenderer>().sortingOrder = 8;
            player.GetComponent<PlayerController>().EnableMovementDisableInputs();
        }
        base.End(isSuccessful);
    }


    public override void StartTask()
    {
        _textScore.text = "0/" + _numOfClicksToDo;
        _angle = 360f;
        _player1 = PlayersDoingTask[0].GetComponent<PlayerController>();
        _player2 = PlayersDoingTask[1].GetComponent<PlayerController>();
        _player1UI = _player1.gameObject.GetComponent<PlayerUI>();
        _player2UI = _player2.gameObject.GetComponent<PlayerUI>();
        _remainingTime = _timeLimit;
        _timer = StartCoroutine(TimerTask());
        IsStarted = true;
    }
    
    void Update()
    {
        if (IsStarted && !IsDone) 
        {
            if(_numOfClicksDone >= _numOfClicksToDo)
            {

                End(true);
            }

            if (_p1Value == 2 || _p2Value == 2)
            {
                _textScore.text = "Penality";
                _textScore.color = Color.red;
                StartCoroutine(Penality());
            }
            if (_p2Value ==  1 && _p1Value == 0)
            {
                _textScore.text = "Penality";
                _textScore.color = Color.red;
                StartCoroutine(Penality());
            }
            if (_p1Value == 1 && _p2Value == 1)
            {
                if (!_hasTamponnageSoundPlayedP2)
                {
                    AudioManager.instance.PlaySFXOS("Tampon", _player2.gameObject.transform.Find("AudioSource").GetComponent<AudioSource>());
                    _hasTamponnageSoundPlayedP2 = true;
                }
                
                _player1UI.DisplayInputToPress(false, "");
                _player2UI.DisplayInputToPress(false, "");
                _numOfClicksDone++;
                _textScore.color = Color.black;
                _textScore.text = _numOfClicksDone +  "/" + _numOfClicksToDo;
                _p1Value = 0;
                _p2Value = 0;
            }
            if (_player1.currentContextName != "" && _data.InputNamesConverter[_player1.currentContextName] == _inputName)
            {
                _p1Value++;
                _player1.currentContextName = "";
            }
            if (_player2.currentContextName != "" && _data.InputNamesConverter[_player2.currentContextName] == _inputName)
            {
                _p2Value++;
                _player2.currentContextName = "";
            }

            if (_p1Value == 0 && _p2Value == 0)
            {
                _hasTamponnageSoundPlayedP1 = false;
                _hasTamponnageSoundPlayedP2 = false;
                _player1UI.DisplayInputToPress(true, _inputName);
            }
            else if(_p1Value == 1 && _p2Value == 0)
            {
                if (!_hasTamponnageSoundPlayedP1)
                {
                    AudioManager.instance.PlaySFXOS("Tampon", _player1.gameObject.transform.Find("AudioSource").GetComponent<AudioSource>());
                    _hasTamponnageSoundPlayedP1 = true;

                }
                
                _player2UI.DisplayInputToPress(true, _inputName);
                _player1UI.DisplayInputToPress(false, "");
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
        if(_remainingTime <= 0)
        {
            End(false );
        }


    }

     IEnumerator Penality()
    {
        _p1Value = 0;
        _p2Value = 0;
        _player1.GetComponent<PlayerController>().DisableAllInputs();
        _player2.GetComponent<PlayerController>().DisableAllInputs();
        AudioManager.instance.PlaySFXOS("QTEFail", RoomTask.AudioSource);
        yield return new WaitForSeconds(5);
        _textScore.color = Color.black;
        _textScore.text = _numOfClicksDone + "/" + _numOfClicksToDo;
        _player1.GetComponent<PlayerController>().DisableMovementEnableInputs();
        _player2.GetComponent<PlayerController>().DisableMovementEnableInputs();

    }



}
