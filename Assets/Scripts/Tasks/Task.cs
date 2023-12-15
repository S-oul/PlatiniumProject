
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class Task : MonoBehaviour
{
    [SerializeField] float rumbleLowForce = .4f;
    [SerializeField] float rumbleHighForce = .1f;
    [SerializeField] float rumbleTime = .4f;

    private List<GameObject> _playersDoingTask = new List<GameObject>();
    [Header("Task variables")]
    [Range(1, 4)][SerializeField] int _numberOfPlayers = 1;
    [Range(1, 5)][SerializeField] int _difficulty = 1;
    [SerializeField] bool _isReplayable = false;
    [SerializeField] bool _addPlayerAtRunTime = false;
    GameObject _player;
    Room _room;
    bool _isDone = false;
    private bool _isStarted = false;

    protected GameManager _gameManager;

    public List<GameObject> PlayersDoingTask { get => _playersDoingTask; set => _playersDoingTask = value; }
    public GameObject PlayerGameObject { get => _player; set => _player = value; }
    public bool IsDone { get => _isDone; set => _isDone = value; }
    public int NumberOfPlayers { get => _numberOfPlayers; set => _numberOfPlayers = value; }

    public bool IsStarted { get => _isStarted; set => _isStarted = value; }

    public Room RoomTask { get => _room; set => _room = value; }
    public int Difficulty { get => _difficulty; set => _difficulty = value; }
    public bool AddPlayerAtRunTime { get => _addPlayerAtRunTime; set => _addPlayerAtRunTime = value; }
    public bool IsReplayable { get => _isReplayable; set => _isReplayable = value; }

    private void Start()
    {
        switch (GameManager.Instance.DayIndex)
        {
            case 0:
                _difficulty = 1;
                break;
            case 1:
                _difficulty = Random.Range(2, 4);
                break;
            case 2:
                _difficulty = Random.Range(4, 6);
                break;
        }
        

    }

    private void Awake()
    {
/*        _room = transform.parent.parent.GetComponent<Room>();
        if (_room == null) { _room = transform.parent.GetComponent<Room>(); }
        if (_room == null) { _room = transform.GetComponent<Room>(); }
        _gameManager = GameManager.Instance;*/
        //print(_gameManager);
    }

    public virtual void Init()
    {
        
        _room = transform.parent.parent.GetComponent<Room>();
        if (_room == null) { _room = transform.parent.GetComponent<Room>(); }
        if (_room == null) { _room = transform.GetComponent<Room>(); }
        _gameManager = GameManager.Instance;
        _room.TaskRoom = this;
        if(_room.WinStateScreen != null)
        {
            _room.WinStateScreen.ChangeValue(WinStateScreen.WinScreenState.Idle);
        }
    }

    

    public virtual void End(bool isSuccessful)
    {
        IsStarted = false;
        if (_isReplayable)
        {
            IsDone = isSuccessful;
        }
        else
        {
            IsDone = true;
        }

        if (isSuccessful)
        {
            OnRoomSuccess();
        }
        else
        {
            
            foreach(GameObject p in _playersDoingTask)
            {
                
                if (p != null)
                {
                    print("realnuggets" + p.name);
                    StartCoroutine(p.GetComponent<PlayerController>().RumbeMeDaddy(rumbleLowForce, rumbleHighForce, rumbleTime));   
                }
            }
            PlayersDoingTask.Clear();
            OnRoomFail();
        }
    }
    public void OnPlayerJoinedTask(GameObject player)
    {
       // Debug.Log("OnPlayerJoinedTask called");  //This func is called 1-3 times at randome. Safegards have been put in place.
        
        if (!IsDone)
        {
            if (AddPlayerAtRunTime)
            {
                if (_isStarted)
                {
                    if (_playersDoingTask.Count < 4)
                    {   
                        //the safegard in question:
                        if (player != _playersDoingTask[PlayersDoingTask.Count - 1]) 
                        {
                            _playersDoingTask.Add(player);
                            GoatTask t = GetComponent<GoatTask>(); 
                            if (t != null)
                            {
                                t.AddPlayer(player);
                            }
                        } 
                        return;
                    }
                }
                else
                {
                    _playersDoingTask.Add(player);
                    _isStarted = true;
                    Init();
                }
            }
            else if (_numberOfPlayers == 1)
            {
                if (!_isStarted)
                {
                    _player = player;
                    _playersDoingTask.Add(player);
                    _isStarted = true;
                    Init();
                }
            }
            else
            {
                _playersDoingTask.Add(player);
                if (_playersDoingTask.Count == _numberOfPlayers)
                {
                    if (!_isStarted)
                    {
                        _isStarted = true;
                        Init();
                    }

                }
            }
        } 
        else
        {
            print("isDone");
        }
    }

    public void OnPlayerLeaveTask(GameObject player)
    {
        if (!IsDone)
        {
            if (AddPlayerAtRunTime)
            {
                if (IsStarted)
                {
                    if (_playersDoingTask.Count > 1)
                    {
                        //the safegard in question:
                        if (player != _playersDoingTask[0])
                        {
                            _playersDoingTask.Remove(player);

                        }
                        return;
                    }

                }
            }
            else
            {
                if (_numberOfPlayers > 1)
                {
                    if (!_isStarted)
                    {

                        _playersDoingTask.Remove(player);
                    }
                }
            }
            
                
            
        }
        
    }

    public virtual void OnplayerExitTask() 
    { 
        
    }
    public void OnRoomSuccess()
    {
        Debug.Log(gameObject.name + " = Success " + _difficulty);
        GameManager.Instance.NumberOfTasksMade++;
        GameManager.Instance.RoomRemainingText.Find("Value").GetComponent<TextMeshProUGUI>().text = (GameManager.Instance.RoomTaskList.Count - GameManager.Instance.NumberOfTasksMade).ToString();

        GameManager.Instance.RoomWin();
        GameManager.Instance.CheckIfDayFinished();
        if (_room.WinStateScreen != null)
        {
            _room.WinStateScreen.ChangeValue(WinStateScreen.WinScreenState.Success);
        }
        AudioManager.Instance.PlaySFXOS("TaskSucceed", _room.AudioSource);
    }
    public void OnRoomFail()
    {
        if(_isReplayable)
        {
            if (_room.WinStateScreen != null)
            {
                _room.WinStateScreen.ChangeValue(WinStateScreen.WinScreenState.Retry);
            }
        }
        else
        {
            if (_room.WinStateScreen != null)
            {
                _room.WinStateScreen.ChangeValue(WinStateScreen.WinScreenState.Fail);
            }
            GameManager.Instance.NumberOfTasksMade++;
            GameManager.Instance.RoomRemainingText.Find("Value").GetComponent<TextMeshProUGUI>().text = (GameManager.Instance.RoomTaskList.Count - GameManager.Instance.NumberOfTasksMade).ToString();

        }
        //Debug.Log(gameObject.name);
        GameManager.Instance.RoomLose();
        GameManager.Instance.CheckIfDayFinished();

        AudioManager.Instance.PlaySFXOS("TaskFail", _room.AudioSource);


    }
    public void OnPlayerExitTaskRoom(GameObject player)
    {
        if (_playersDoingTask.Contains(player) && !IsStarted)
        {
            _playersDoingTask.Remove(player);
        }
    }
}

public interface ITimedTask
{
    float _givenTime { get; }
}




